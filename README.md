# .NET Framework and C# - Project work - Group 1
## Table of Contents
<ol>
  <li>
    <a href="#about-the-project">About The Project</a>
    <ul>
      <li><a href="#contributors">Contributors</a></li>
      <li><a href="#built-with">Built With</a></li>
    </ul>
  </li>
  <li>
    <a href="#setup-process">Setup Process</a>
  </li>
  <li>
    <a href="#description">Description</a>
    <ul>
      <li><a href="#er-model">ER Model</a></li>
      <li><a href="#dump-file">Dump File</a></li>
    </ul>
  </li>
</ol>
<br/>

<!-- ABOUT THE PROJECT -->
## About The Project
This repository contains the group project for the course **.Net Framework and C#** at the university of applied sciences in Darmstadt, Germany. 

The application consists of a backend [WebAPI](https://dotnet.microsoft.com/en-us/apps/aspnet/apis), a frontend [WPF](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/?view=netdesktop-6.0)  client and a [postgreSQL](https://www.postgresql.org/docs/) database for data storage. 
### Contributors
Student members of project group:
- [Toubeyas (Abel, Tobias)](https://github.com/Toubeyas) 
- [Iron-Mike-Tyson (de Riese-Meyer, Kevin)](https://github.com/Iron-Mike-Tyson) 
- [T-hai (Quang Thai Vu)](https://github.com/T-hai) 

Lecturer:
- [alaluuk (Alaluukas, Pekka)](https://github.com/alaluuk)
### Built With
This chapter contains a list of all technologies that are being used in this project. 
#### Frameworks
- [.NET 6.0](https://docs.microsoft.com/en-us/dotnet/?WT.mc_id=dotnet-35129-website) for class library and frontend
- [ASP.NET Core 6.0](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-6.0) for backend
- [MSTest](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest) for unit testing

#### Security
- Authorization of WebAPI with [JWT](https://jwt.io/introduction)
#### Services
- [Central postgresql server](https://code.fbi.h-da.de/lab-docs/db/-/wikis/postgresql/Zentraler-PostgreSQL-Server) from [h_da](https://h-da.de/en/) as a database (german documentation)
- [The Movie Database API](https://developers.themoviedb.org/3) is used to retrieve initial Media and Series objects and their Streaming Availability
  
<!-- Setup Process -->
## Setup Process
(how to install the application, so that a new developer can start to work)

//TODO

<!-- DESCRIPTION -->
## Description
The StreamKing Application is used to get Streaming Availability Informations for many Movies & Series in the Regions 
USA, Germany and Finland. You are also able 
to save these to your own personal Watchlist which is linked to your Account and is then being saved in our central 
Database.
### ER Model
This is the ER-Model for the PSQL Database, which was created with the help of the SAP PowerDesigner. 
We didn't use the automatically created versions, e.g. from pgAdmin, because they weren't really readable and 
we had more freedom with this tool.
<img src="./docs/er_model.png"/>

### Dump File
[SQL Dump File](docs/StreamKingSQLDumpFile.sql) (From: 09.09.2022 00:10 AM) which was created with the pgAdmin PSQL Tool.