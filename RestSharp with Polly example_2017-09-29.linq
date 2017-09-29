<Query Kind="Program">
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <NuGetReference>Polly-Signed</NuGetReference>
  <NuGetReference>RestSharpSigned</NuGetReference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>RestSharp</Namespace>
  <Namespace>Polly</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	var requestUri = new Uri("http://test.test/test");
	var request =
		new RestRequest(Method.POST)
			.AddHeader("Cache-Control", "no-cache")
			.AddHeader("Content-Type", "application/json;charset=utf-8")
			.AddJsonBody(
			new
			{
				No = "123456"
			});
	var client = new RestClient(requestUri);
	
	var response = RestSharpRetryHelper.ExecuteWithRetry(
		() => client.Execute(request),
		2,
		retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
		(exception, timeSpan, retryCount, context) =>
		{
			exception.Dump();
			timeSpan.Dump();
			retryCount.Dump();
			context.Dump();
		});
			
	response.Dump();
}

public static class RestSharpExtension
{
	public static bool IsSuccessful(this IRestResponse response)
	{
		return response.StatusCode.IsSuccessStatusCode()
				&& response.ResponseStatus == ResponseStatus.Completed;
	}

	public static bool IsSuccessStatusCode(this HttpStatusCode responseCode)
	{
		int numericResponse = (int)responseCode;
		return numericResponse >= 200
				&& numericResponse <= 299;
	}
}

public class RestSharpRetryHelper
{
	/// <summary>
	/// 使用重試邏輯執行非同步方法。
	/// </summary>
	/// <param name="func">要執行的非同步方法。</param>
	/// <param name="maxAttempts">最大重試次數。</param>
	/// <param name="retryIntervalProvider">重試時間間隔提供者。</param>
	/// <param name="onAttemptFailed">重試失敗時執行的回呼方法。</param>
	/// <returns>這個 <see cref="Task"/> 產生的結果。</returns>
	/// <exception cref="System.ArgumentNullException">func or retryIntervalProvider</exception>
	/// <exception cref="System.ArgumentOutOfRangeException">maxAttempts - 最大重試次數不能小於 1。</exception>
	public static IRestResponse ExecuteWithRetry(
		Func<IRestResponse> func,
		int maxAttempts,
		Func<int, TimeSpan> retryIntervalProvider,
		Action<DelegateResult<IRestResponse>, TimeSpan, int, Context> onAttemptFailed = null)
	{
		var paramsChecklist = new List<KeyValuePair<Func<bool>, Action>>(3)
			{
				new KeyValuePair<Func<bool>, Action>(
					() => func == null,
					() => { throw new ArgumentNullException(nameof(func)); }),
				new KeyValuePair<Func<bool>, Action>(
					() => maxAttempts < 1,
					() => { throw new ArgumentOutOfRangeException(nameof(maxAttempts), maxAttempts, "最大重試次數不能小於 1。"); }),
				new KeyValuePair<Func<bool>, Action>(
					() => retryIntervalProvider == null,
					() => { throw new ArgumentNullException(nameof(retryIntervalProvider)); })
			};

		paramsChecklist.FirstOrDefault(f => f.Key()).Value?.Invoke();

		if (onAttemptFailed == null)
		{
			onAttemptFailed = (exception, timeSpan, retryCount, context) => { };
		}

		var result = Policy.Handle<Exception>()
			 .OrResult<IRestResponse>(response => response.IsSuccessful() == false)
			 .WaitAndRetry(maxAttempts, retryIntervalProvider, onAttemptFailed)
			 .Execute(() => func());

		return result;
	}
}