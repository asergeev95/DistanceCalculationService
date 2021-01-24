# DistanceCalculationService

The purpose of this service is to calculate the distance in miles between two airports. Each airport is identified using IATA code. 

You can launch a service using either in a Docker or just with Visual Studio/ JetBrains Rider IDE.

Service have Redis as an external dependency. You should have Redis running somewhere. If your choose Docker method of running Redis will be installed automatically. 


## Docker 
1. clone a project to your local directory: `git clone https://github.com/asergeev95/DistanceCalculationService`
2. Open `docker-compose.yml` and change `<your current IP>` to your current IP (to explore current IP execute in command line `ipconfig`). In redis service description you can change first 6379 port as well but in this case you will need to change it in redis configuration of the application service to the same. 
3. Execute in command line: `cd DistanceCalculationService`
4. Execute in command line: `docker-compose up -d`
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
