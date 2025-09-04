using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.Logging;

namespace YmmeUtil.Ymm4;

/// <summary>
/// プラグイン由来の例外を捕捉して処理するためのユーティリティ
/// </summary>
/// <example>
/// <code>
/// _monitor = PluginExceptionMonitor.Initialize(typeof(MyPlugin).Assembly, logger);
/// </code>
/// </example>
public sealed class PluginErrorHandler
{
	readonly string _targetAssemblyName = string.Empty;
	readonly ILogger _logger;

	private PluginErrorHandler(
		Assembly targetAssembly,
		ILogger logger
	)
	{
		_targetAssemblyName =
			targetAssembly.GetName().Name ?? string.Empty;
		_logger = logger;

		AppDomain.CurrentDomain.UnhandledException +=
			HandleUnhandledException();

		TaskScheduler.UnobservedTaskException +=
			HandleUnobservedTaskException();

		if (Application.Current is null)
		{
			return;
		}
		Application.Current.DispatcherUnhandledException +=
			HandleDispatchExceptionEvent();
	}

	DispatcherUnhandledExceptionEventHandler HandleDispatchExceptionEvent() =>
		(s, e) =>
		{
			if (IsFromTargetAssembly(e.Exception))
			{
				HandlePluginException(
					e.Exception,
					nameof(
						Application
							.Current
							.DispatcherUnhandledException
					)
				);
				e.Handled = true;
			}
		};

	EventHandler<UnobservedTaskExceptionEventArgs> HandleUnobservedTaskException() =>
		(s, e) =>
		{
			if (IsFromTargetAssembly(e.Exception))
			{
				HandlePluginException(
					e.Exception,
					nameof(
						TaskScheduler.UnobservedTaskException
					)
				);
				e.SetObserved();
			}
		};

	UnhandledExceptionEventHandler
	HandleUnhandledException() =>
		(s, e) =>
		{
			if (
				e.ExceptionObject is Exception ex
				&& IsFromTargetAssembly(ex)
			)
			{
				HandlePluginException(
					ex,
					nameof(
						AppDomain
							.CurrentDomain
							.UnhandledException
					)
				);
			}
		};

	/// <summary>
	/// 指定したプラグインアセンブリ由来の例外を捕捉して処理するハンドラを初期化する
	/// </summary>
	/// <param name="targetAssembly"></param>
	/// <param name="logger"></param>
	/// <returns></returns>
	/// <seealso cref="YmmeUtil.Common.AssemblyUtil"/>
	public static PluginErrorHandler Initialize(
		Assembly targetAssembly,
		ILogger logger
	)
	{
		return new PluginErrorHandler(
			targetAssembly,
			logger
		);
	}

	bool IsFromTargetAssembly(Exception? ex)
	{
		while (ex is not null)
		{
			// メソッドのアセンブリで判定
			if (IsTargetAssemblyException(ex))
			{
				return true;
			}

			// スタックトレース文字列で判定（保険）
			if (IsAssemblyInStackTrace(ex))
			{
				return true;
			}

			ex = ex.InnerException;
		}
		return false;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		bool IsAssemblyInStackTrace(Exception ex)
		{
			return (ex.StackTrace ?? string.Empty).Contains(
				_targetAssemblyName,
				StringComparison.Ordinal
			);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		bool IsTargetAssemblyException(Exception ex)
		{
			return string.Equals(
				ex.TargetSite?.DeclaringType?.Assembly?.GetName().Name,
				_targetAssemblyName,
				StringComparison.Ordinal
			);
		}
	}

	static readonly Action<ILogger, string, string, Exception?> LogPluginException =
		LoggerMessage.Define<string, string>(
			LogLevel.Error,
			new EventId(1, nameof(HandlePluginException)),
			"[{TargetAssembly}][{Source}] 由来の例外を捕捉"
		);

	void HandlePluginException(Exception ex, string source)
	{
		var msg = $"""
			[{_targetAssemblyName}][{source}]
				{_targetAssemblyName} 由来の例外を捕捉: {ex.Message}
			""";
		Debug.WriteLine(msg);
		LogPluginException(
			_logger,
			_targetAssemblyName,
			source,
			ex
		);
	}
}
