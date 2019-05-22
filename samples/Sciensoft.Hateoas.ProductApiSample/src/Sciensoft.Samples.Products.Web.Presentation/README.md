# The Assessment

## RESTful API

The API make usage of a open-source library called `Sciensoft.Hateoas` for achieving REST Uniform-Interface HATEOAS constraint. 
This library is a open-source project I've been working as a side project, for more information please visit my project on Git - [Sciensoft.Hateoas](https://github.com/higtrollers/Sciensoft.Hateoas).

### HTTP Messages

Below I'm describing the raw messages used for retrieving and updating the state of products.

#### GET

Get all products.

```
GET /api/products HTTP/1.1
Host: localhost:5000
Accept: application/json
cache-control: no-cache
```

Get single product.

```
GET /api/products/s003 HTTP/1.1
Host: localhost:5000
Accept: application/json
cache-control: no-cache
```

#### POST

Create a new product.

```
POST /api/products HTTP/1.1
Host: localhost:5000
Content-Type: application/json
cache-control: no-cache
{
	"code": "p_636_c",
	"name": "Product 658",
	"price": {
		"value": 1050,
		"approved": true
	}
}
```

#### PUT

Update existing product.

```
PUT /api/products/s003 HTTP/1.1
Host: localhost:5000
Content-Type: application/json
If-Match: E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B855
cache-control: no-cache
{
	"code": "s003",
	"name": "Product 3 - v402",
	"price": {
		"value": 1050,
		"approved": true
	}
}
```

#### PATCH

```
PATCH /api/products/s003 HTTP/1.1
Host: localhost:5000
Content-Type: application/json
If-Match: E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B855
cache-control: no-cache
[
  {
    "op": "replace",
    "path": "/name",
    "value": "Product Updated 3"
  },
  {
  	"op": "replace",
  	"path": "/price/value",
  	"value": 341.49
  }
]
```

#### DELETE

```
DELETE /api/products/s001 HTTP/1.1
Host: localhost:5000
Content-Type: application/json
cache-control: no-cache
{
  "reason": "Some reason for deleting the product."
}
```


## Frontend UI

### Operations

The UI implements products list, create, update and delete operations.

### Boostrap Theme

This assessment uses a free Bootstrap theme called **Business Frontpage**, for more information about the theme visit [Bootstrap - Business Frontpage](https://startbootstrap.com/templates/business-frontpage) website.