# Logging.Unity.Inteception [![Build Status](https://ci.appveyor.com/api/projects/status/32r7s2skrgm9ubva?svg=true)](https://ci.appveyor.com/project/semashkinvg/logging-unity-inteception)

## Example
```
var uc = new UnityContainer();
uc.AddNewExtension<Interception>();

uc.RegisterType<IDummyService, DummyService>(
    new Interceptor<InterfaceInterceptor>(), new InterceptionBehavior<LoggingInterceptionBehavior>());
```

## Customization

To change method description, please inherite from `LoggingInterceptionBehavior` and override `BuildMethodInvocationDescr`. By default, it uses [MethodInvocationExtensions](https://github.com/semashkinvg/Logging.Unity.Inteception/blob/master/src/Logging.Unity.Interception/MethodInvocationExtensions.cs)