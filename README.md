# DistanceCalculationService

The purpose of this service is to calculate distance in miles between two airports. Each an airport is identified using IATA code. 

You can launch a services using either a Docker or just Visual Studio/ JetBrains Rider IDE.

Services has Redis as an external dependency. You should have Redis running somewhere. If your choose Docker method of running Redis will be installed automatically. 


## Docker 
1. clone a project to your local directory
2. Open `docker-compose.yml` and change `<your current IP>` to your current IP
3. in command line: `cd DistanceCalculationService`
4. in command line: `docker-compose up -d`
5. Launch a browser and go to `localhost:8080`
6. You will see a swagger page and method for distance calculation
7. Click `Try it out`, put your input data (i.e. iataCode = ovb, destIataCode = ams) and click `Execute`


## IDE
You should have Redis installed on your machine or you can use any other one. 
1. clone a project, open solution
2. in appsettings.json put correct configuration for Redis (`"Redis": "localhost:6379"`)
3. launch `DCS.Host` project
4. You will see a swagger page and method for distance calculation
5. Click "Try it out", put your input data and click "Execute"
