Quick Start:

1) Create the "Blazor Server" project with Authetication Type = Individual Accounts

2) Apply the 00000000000000_CreateIdentitySchema migration to the aspnet-BlazorAuth-9F8F37E8-C7E8-4D29-BFE6-47204A65FA44 database:
PM> Update-Database

3) Create and confirm new accounts by using the Identity/Account/Register form:

Email: admin@gmail.com
Password: 1*234aB

Email: user@gmail.com
Password: 1*234aC

Email: guest@gmail.com
Password: 1*234aD

See also: https://docs.microsoft.com/en-us/aspnet/core/blazor/security/?view=aspnetcore-5.0