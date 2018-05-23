# There are 2 environments: 
Development
Production
To change environment, right click project and choice Properties. Change ASPNETCORE_ENVIRONMENT and must restart Visual Studio.

# To clone all function of a entity: 

Step 1: Create a Entity. 

After creating entity, add DbSet on DatabaseContext.cs. 

Add a new migration with command: `add-migration EntityName_YourNote`. We should name the migration with prefix EntityName. 

Run command `update-database` to change database.

Step 2: Create a Dto on Models Folder.

We should name the dto with suffix Dto.

Step 3: Config Auto Mapper on MappingProfile.cs. example: 
 `CreateMap<ProviderEntity, ProviderDto>();`

Step 4: Create a IRepository and Repository on Services folder.

Step 5: Map IRepository with Repository on Startup.cs. Example:  
`services.AddTransient<IProviderRepository, ProviderRepository>();`

Step 6: Create a Controller.

