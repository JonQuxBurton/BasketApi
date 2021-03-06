# Basket REST API

## Overview

This document describes the prototype of the Basket REST API. The design of the API is modeled on the following references:

["Building a RESTful API with ASP.NET Core" Pluralsight course by Kevin Dockx](https://app.pluralsight.com/library/courses/asp-dot-net-core-restful-api-building/table-of-contents)

[API Design by Microsoft](https://docs.microsoft.com/en-us/azure/architecture/best-practices/api-design)

[REST API v3 by Github](https://developer.github.com/v3/)

All API access is over HTTP. All data is sent and received as JSON.

## Features

The following is a list of all the features which the API contains.
Note: the URLs below should be prefixed with the host and port, for example: http://localhost:57332/api

All these requests are contained on the following Postman collection: https://github.com/JonQuxBurton/BasketApi/blob/master/BasketApi.postman_collection.json


### Create a new Basket
```
POST /baskets
```

Response
```
Status: 201 Created
Headers
Location: http://localhost:57332/api/Baskets/3080d143-6412-45cb-a82f-798ad1065177
{
    "id": "3080d143-6412-45cb-a82f-798ad1065177",
    "items": []
}	
```

### Get a Basket
Gets a Basket and its Items.
```
GET /baskets/:basketId
```
Response
```
Status: 200 OK
{
    "id": "3080d143-6412-45cb-a82f-798ad1065177",
    "items": []
}
```
	
### Clear a Basket
Clears all the items from a Basket.

```
POST /baskets/:basketId/clear
```
Response
```
Status: 204 No Content
```

### Add an Item to a Basket
Adds an Item and Quantity to a Basket. If the Item is already in the Basket, the Quantity will be updated.
```
PUT /baskets/:basketId
{
	"code": "Arduino",
	"quantity": 42
}
Accept-Type: application/json
```

Response 
```
Status: 204 No Content
```

### Get an Item from a Basket
Gets an Item from a Basket.
```
GET /baskets/:basket/items/:item
```

Response
```
Status: 200 OK
{
    "code": "Arduino",
    "quantity": 42
}
```

### Remove an Item from a Basket
Removes an Item from a Basket. If the Item is not found, the same status code of 204 No Content is returned so this request idempotent.
```
DELETE /baskets/:basket/items/:item
```

Response
```
Status: 204 No Content
```

### Get all the Items in a Basket
```
GET /basket:basket
```
Response
```
Status: 200 OK
[
    {
        "code": "Arduino",
        "quantity": 42
    },
    {
        "code": "BBC micro:bit",
        "quantity": 101
    }
]
```

### Check the Status of the API
Returns 200 OK, can be used as a basic check for connectivity and if the API is available.
```
GET /status
```
Response
```
Status: 200 OK
```
		
## Projects

The projects in the solution are as follows:

### BasketApi
The Basket REST API. This is currently setup to use an InMemory datastore. In production this can be replaced by registering the production data store in the composition root (Startup.cs).

### BasketApiClient
A project which allows clients to use the API more easily.

### BasketApiConsoleApp
A sample console app which uses the BasketApiClient to exercise the API.

---

## Enhancements

Some suggestions for future enhancements:

### HATEOAS
The current version of the API is at level 2 on the [Richardson Maturity Model](https://martinfowler.com/articles/richardsonMaturityModel.html). To reach level 3, HATEOAS could be added. This is where hyperlinks are returned in the responses that describes the operations available on each of resource. The system acts as a state machine and clients can use these links to move from one state to another. This would make the API more evolveable without needing to update all the clients. For example the [Siren](https://github.com/kevinswiber/siren) standard could be used.

### Rate Limiting
To make the API more robust, rate limiting could be added by using the [AspNetCoreRateLimit middleware](https://github.com/stefanprodan/AspNetCoreRateLimit).

### AutoFixture

The [AutoFixture](https://github.com/AutoFixture/AutoFixture) library could be added which reduces the amount of test fixture setup code. Making develpoment more productive and the unit tests more refactoring safe. It now supports .NET Core since January 2018.

### Update 18/03/2017

### NSpec

The [NSpec](http://nspec.org/) could be used (instead of AutoFixture), which allows tests to be written in a more intuitive style so they read more like a specification. 

### Validation

The ItemsController.Put() method should validate that the Item being added does not have a negative quantity and return 422 Unprocessable entity if it does.
