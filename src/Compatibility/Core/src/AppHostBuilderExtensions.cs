using System;
using System.Collections.Generic;
using Microsoft.Maui.Hosting;

namespace Microsoft.Maui.Controls.Compatibility
{
	public static class AppHostBuilderExtensions
	{
		public static IAppHostBuilder RegisterCompatibilityRenderers(this IAppHostBuilder builder)
		{
			// This won't really be a thing once we have all the handlers built
			var defaultHandlers = new List<Type>
			{
				  typeof(Button) ,
		  typeof(Editor),
				  typeof(Entry) ,
				  typeof(ContentPage) ,
				  typeof(Page) ,
				  typeof(Label) ,
				  typeof(Slider),
				  typeof(Switch)
			};

#if !WINDOWS
			Forms.RegisterCompatRenderers(
				new[] { typeof(RendererToHandlerShim).Assembly },
				typeof(RendererToHandlerShim).Assembly,
				(controlType) =>
				{
					foreach (var type in defaultHandlers)
					{
						if (type.IsAssignableFrom(controlType))
							return;
					}

					builder.RegisterHandler(controlType, typeof(RendererToHandlerShim));
				});
#endif

			return builder;
		}

		public static IAppHostBuilder RegisterCompatibilityRenderer(
			this IAppHostBuilder builder,
			Type controlType,
			Type rendererType)
		{
			// register renderer with old registrar so it can get shimmed
			// This will move to some extension method
			Microsoft.Maui.Controls.Internals.Registrar.Registered.Register(
				controlType,
				rendererType);
#if !WINDOWS
			builder.RegisterHandler(controlType, typeof(RendererToHandlerShim));
#endif

			return builder;
		}

		public static IAppHostBuilder RegisterCompatibilityRenderer<TControlType, TMauiType, TRenderer>(this IAppHostBuilder builder)
			where TMauiType : IFrameworkElement
		{
			// register renderer with old registrar so it can get shimmed
			// This will move to some extension method
			Controls.Internals.Registrar.Registered.Register(
				typeof(TControlType),
				typeof(TRenderer));
#if !WINDOWS
			builder.RegisterHandler<TMauiType, RendererToHandlerShim>();
#endif

			return builder;
		}

		public static IAppHostBuilder RegisterCompatibilityRenderer<TControlType, TRenderer>(this IAppHostBuilder builder)
			where TControlType : IFrameworkElement =>
				builder.RegisterCompatibilityRenderer<TControlType, TControlType, TRenderer>();
	}
}