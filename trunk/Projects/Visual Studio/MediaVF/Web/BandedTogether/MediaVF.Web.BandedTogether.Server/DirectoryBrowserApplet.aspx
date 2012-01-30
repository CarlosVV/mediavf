<html>
    <head>
        <title>Music Directory Browser</title>
        <script type="text/javascript">

            function setBands(bandText) {

                try {
                    window.opener.handleMessageFromPopup(window.name, 'BandsFound', bandText);
                } catch (ex) {
                    alert(ex.Message);
                }

                window.close();
            }
        </script>
    </head>
    <body>
        <applet code="directoryBrowser.DirectoryBrowserApplet" archive="DirectoryBrowser.jar" width="500" height="300" />
    </body>
</html>