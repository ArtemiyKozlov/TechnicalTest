# Blackdot Solutions Technical Test

To run the app run "dotnet run -p Bds.TechTest"
To run tests run "dotnet test"

ASP.Net Core 3.1 Razor Page application.

The app configured to combine the searches from Bing.com and Yahoo.com
Search results parsed from HTML responses of "search?q=text" endpoints.
This approach works fine for a few concurrnet sessions, 
but it will be hard to scale because search engines might easily detect automated requests and block them.
Generally speaking, modern search engines are extremely complex, they track your search activity, provide smart autocomplete, and much more.
I haven't come up with a solution for passing the user's identity to the engine without using API.

Due to lack of time, there are quite a few tests. Views with styles should be improved.

Error handling - TODO.
Pagination - backend - done : UI - TODO