# Running the application
Build

`docker build . -t shortener -f UrlShortener.App/Dockerfile`

Run

`docker run shortener -p 8443:443`

Application should be running on port 8443