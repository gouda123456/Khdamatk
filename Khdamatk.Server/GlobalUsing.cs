
global using System.ComponentModel;
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using Microsoft.AspNetCore.Identity;

global using Khdamatk.Server.Statics.SeedingData;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;

global using Khdamatk.Server.ResultPattern;
global using Khdamatk.Server.Statics.Consts;

global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;

global using System.Reflection;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

global using Khdamatk.Server.Data.Entities.Identity;
global using Khdamatk.Server.Data.Entities.Interaction;
global using Khdamatk.Server.Data.Entities.Catalog;
global using Khdamatk.Server.Data.Entities.Financial;
global using Khdamatk.Server.Data.Entities.Operations;


global using System.Text;
global using Khdamatk.Server.Data;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.IdentityModel.Tokens;


global using System.Security.Cryptography;
global using Khdamatk.Server.Services.Interfaces;
global using Khdamatk.Server.Statics.Settings;
global using Microsoft.Extensions.Options;
global using Newtonsoft.Json.Linq;


global using FluentValidation;
global using Khdamatk.Server.Statics.Regex;



global using Khdamatk.Server.Contracts.Authentications;


global using static Khdamatk.Server.ResultPattern.ResultPattern;
global using Mapster;





global using Khdamatk.Server.Helper;
global using Khdamatk.Server.MiddleWares;
global using MapsterMapper;
global using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
global using Khdamatk.Server.Statics.Errors;
global using Microsoft.AspNetCore.WebUtilities;
global using MimeKit;
global using Khdamatk.Server.Services.Implementations;



global using Khdamatk.Server.Authentication.Fillter;
global using Microsoft.AspNetCore.Authorization;