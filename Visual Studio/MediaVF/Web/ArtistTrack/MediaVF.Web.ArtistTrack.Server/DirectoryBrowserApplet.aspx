<html>
    <head>
        <title>Music Directory Browser</title>
        <script type="text/javascript">

            function setArtists(artistsText) {

                try {
                    window.opener.handleMessageFromPopup(window.name, 'ArtistsFound', artistsText);
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