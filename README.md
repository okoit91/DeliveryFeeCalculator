
Project uses .NET 8.0 and Postgres.
Api endpoints can be seen from Swagger.


1) open the DeliveryFeeCalculator folder

Run these commands to get the database migrated and Docker container running.

~~~bash

dotnet ef migrations  --project App.DAL.EF --startup-project WebApp add Initial

docker compose up -d

dotnet ef database    --project App.DAL.EF --startup-project WebApp update

dotnet run --project WebApp/WebApp.csproj

~~~

2) open the DeliveryFeeCalculatorFront folder and run following commands:

~~~bash

npm install
npm run dev

~~~

Admin user is created during seeding, please use the account to reach crud controllers and use the post/put functions.

Email: admin@admin.com
PW: Admin123.


On each application restart, weather data will be fetched again. Otherwise every 15th minute of the hour.
Weathers controller does not have crud pages.

Upon registering a new account, client role will be given and visually only delivery fee calculator is available.
Also get requests to all controllers are allowed for the client role.

Most of the controllers give back the whole data for the test application.
In the future pagination could be used for better performance.

For this project base solution might seem like an overkill, but I think its reasonable to implement it early.
