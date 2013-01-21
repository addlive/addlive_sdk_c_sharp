AddLive native SDK .NET bindings
===============

Build instructions
---------------

Microsoft Visual Studio 2010 or higher should be used. Just start solution and build SDK bindings (ADL project), Test application (sample\_app project).


How to use bindings
---------------

- Build ADL project from ADL solution (as described above)
- Download AddLive native SDK from https://s3.amazonaws.com/api.addlive.com/beta/AddLive\_sdk-win.zip
- Unpack AddLive\_sdk-win directory to your application's directory. ADL.dll should be placed on the same level in FS. So, for example, if you have your own application app.exe which uses AddLive SDK, then file system layout can be like this:

- [parent dir]
    - app.exe
    - ADL.dll
    - AddLive\_sdk-win
        - adl_sdk.dll
        - libs
            - AddLiveService.dll
            - ...
        - ...


Sample application (sample\_app project) should be configured in a similar way.
