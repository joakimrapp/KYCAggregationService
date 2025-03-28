# KYC Aggregation Service

A know your customer (KYC) process is a procedure that financial institutions and other businesses follow to verify the identity of their customers. This process helps to prevent money laundering, terrorist financing, and fraud. It typically includes collecting, verifying, and documenting specific information about individuals or entities that engage in business activities with the organization.

Below, you will find the instructions for getting started with you task.
Please read them carefully to avoid unexpected issues. Best of luck!

## Mandatory steps before you get started

1. Use one or multiple .NET Core projects but in one single solution.
2. Initialize the project folders as a git repository.

## The Task

Your task is to build a backend service that implements the KYC Aggregation Service API described below. It needs to integrate with the [Customer Data API](https://customerdataapi.azurewebsites.net/api) to aggregate historical customer and KYC data

<details>
<summary>KYC Aggregation Service API Specification</summary>

```json
{
    "openapi": "3.0.1",
    "info": {
      "title": "KYC Aggregation API Service",
      "description": "API for aggregating KYC (Know Your Customer) data",
      "version": "1.0.0"
    },
    "paths": {
      "/kyc-data/{ssn}": {
        "get": {
          "summary": "Get aggregated KYC data",
          "operationId": "GetAggregatedKycData",
          "tags": [
            "KYC Aggregation"
          ],
          "parameters": [
            {
              "name": "ssn",
              "in": "path",
              "required": true,
              "schema": {
                "type": "string"
              },
              "description": "Social Security Number of the customer"
            }
          ],
          "responses": {
            "200": {
              "description": "Successful response",
              "content": {
                "application/json": {
                  "schema": {
                    "$ref": "#/components/schemas/AggregatedKycData"
                  },
                  "example": {
                    "ssn": "19801115-1234",
                    "first_name": "Lars",
                    "last_name": "Larsson",
                    "address": "Smågatan 1, 123 22 Malmö",
                    "phone_number": "+46 70 123 45 67",
                    "email": "lars.larsson@example.com",
                    "tax_country": "SE",
                    "income": 550000
                  }
                }
              }
            },
            "404": {
              "description": "Customer data not found",
              "content": {
                "application/json": {
                  "example": {
                    "error": "Customer data not found for the provided SSN."
                  }
                }
              }
            },
            "500": {
              "description": "Internal server error",
              "content": {
                "application/json": {
                  "example": {
                    "error": "An unexpected error occurred while processing the request."
                  }
                }
              }
            }
          }
        }
      }
    },
    "components": {
      "schemas": {
        "AggregatedKycData": {
          "type": "object",
          "properties": {
            "ssn": {
              "type": "string",
              "example": "19830115-1234"
            },
            "first_name": {
              "type": "string",
              "example": "Sven"
            },
            "last_name": {
              "type": "string",
              "example": "Svensson"
            },
            "address": {
              "type": "string",
              "example": "Storgatan 1, 111 22 Stockholm"
            },
            "phone_number": {
              "type": "string",
              "example": "+46 70 123 45 67"
            },
            "email": {
              "type": "string",
              "example": "sven.svensson@example.com"
            },
            "tax_country": {
              "type": "string",
              "example": "SE"
            },
            "income": {
              "type": "integer",
              "nullable": true,
              "example": 550000
            }
          },
          "required": [
            "first_name",
            "last_name",
            "address",
            "ssn",
            "tax_country"
          ]
        }
      }
    }
  }
```
</details>
<details>
<summary>Customer data API Specification</summary>

```json
{
  "openapi": "3.0.0",
  "info": {
    "title": "Customer Data API",
    "version": "1.0.0",
    "description": "API for retrieving customer personal details, contact information, and KYC form data."
  },
  "servers": [
    {
      "url": "https://customerdataapi.azurewebsites.net/api",
      "description": "Production server"
    }
  ],
  "paths": {
    "/personal-details/{ssn}": {
      "get": {
        "summary": "Get customer personal details",
        "parameters": [
          {
            "in": "path",
            "name": "ssn",
            "required": true,
            "schema": {
              "type": "string"
            },
            "description": "Social Security Number of the customer"
          }
        ],
        "responses": {
          "200": {
            "description": "Successful response",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/PersonalDetails"
                }
              }
            }
          },
          "404": {
            "description": "Customer not found"
          },
          "400": {
            "description": "Invalid SSN format"
          }
        }
      }
    },
    "/contact-details/{ssn}": {
      "get": {
        "summary": "Get customer contact details",
        "parameters": [
          {
            "in": "path",
            "name": "ssn",
            "required": true,
            "schema": {
              "type": "string"
            },
            "description": "Social Security Number of the customer"
          }
        ],
        "responses": {
          "200": {
            "description": "Successful response",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/ContactDetails"
                }
              }
            }
          },
          "404": {
            "description": "Customer not found"
          },
          "400": {
            "description": "Invalid SSN format"
          }
        }
      }
    },
    "/kyc-form/{ssn}/{asOfDate}": {
      "get": {
        "summary": "Get customer KYC form data",
        "parameters": [
          {
            "in": "path",
            "name": "ssn",
            "required": true,
            "schema": {
              "type": "string"
            },
            "description": "Social Security Number of the customer"
          },
          {
            "in": "path",
            "name": "asOfDate",
            "required": true,
            "schema": {
              "type": "string",
              "format": "date"
            },
            "description": "The date for which to retrieve the KYC form data"
          }
        ],
        "responses": {
          "200": {
            "description": "Successful response",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/KYCForm"
                },
                "example": {
                  "items": [
                    {
                      "key": "tax_identification_number",
                      "value": "123-45-6789"
                    },
                    {
                      "key": "date_of_birth",
                      "value": "1980-01-15"
                    },
                    {
                      "key": "nationality",
                      "value": "Denmark"
                    },
                    {
                      "key": "occupation",
                      "value": "Software Engineer"
                    },
                    {
                      "key": "source_of_funds",
                      "value": "Employment Income"
                    },
                    {
                      "key": "politically_exposed_person",
                      "value": "No"
                    },
                    {
                      "key": "annual_income",
                      "value": "500000"
                    },
                    {
                      "key": "expected_transaction_volume",
                      "value": "100000"
                    },
                    {
                      "key": "purpose_of_account",
                      "value": "Savings and Investments"
                    },
                    {
                      "key": "risk_profile",
                      "value": "Medium"
                    },
                    {
                      "Key": "tax_country",
                      "Value": "DK"
                    },
                    {
                      "Key": "kyc_registration_date",
                      "Value": "1997-10-01"
                    }
                  ]
                }
              }
            }
          },
          "404": {
            "description": "Customer or KYC form not found"
          },
          "400": {
            "description": "Invalid SSN format or date"
          }
        }
      }
    }
  },
  "components": {
    "schemas": {
      "PersonalDetails": {
        "type": "object",
        "properties": {
          "first_name": {
            "type": "string"
          },
          "sur_name": {
            "type": "string"
          }
        }
      },
      "ContactDetails": {
        "type": "object",
        "properties": {
          "address": {
            "type": "array",
            "items": {
              "$ref": "#/components/schemas/Address"
            }
          },
          "emails": {
            "type": "array",
            "items": {
              "$ref": "#/components/schemas/Email"
            }
          },
          "phone_numbers": {
            "type": "array",
            "items": {
              "$ref": "#/components/schemas/PhoneNumber"
            }
          }
        }
      },
      "Address": {
        "type": "object",
        "properties": {
          "street": {
            "type": "string"
          },
          "city": {
            "type": "string"
          },
          "state": {
            "type": "string"
          },
          "postal_code": {
            "type": "string"
          },
          "country": {
            "type": "string"
          }
        }
      },
      "Email": {
        "type": "object",
        "properties": {
          "preferred": {
            "type": "boolean"
          },
          "email_address": {
            "type": "string"
          }
        }
      },
      "PhoneNumber": {
        "type": "object",
        "properties": {
          "preferred": {
            "type": "boolean"
          },
          "number": {
            "type": "string"
          }
        }
      },
      "KYCForm": {
        "type": "object",
        "properties": {
          "items": {
            "type": "array",
            "items": {
              "$ref": "#/components/schemas/KYCItem"
            }
          }
        }
      },
      "KYCItem": {
        "type": "object",
        "properties": {
          "key": {
            "type": "string"
          },
          "value": {
            "type": "string"
          }
        }
      }
    }
  }
}
```
</details>

#### Test Data
There is test data available for the following SSNs:
```
19800115-1234
19900220-5678
19751230-9101
19850505-4321
19951212-3456
```

### Solution expectations

- Implement persistent caching. The first time the data is fetched from the remote API, it should be persisted. All subsequent requests to fetch the same data should be served from the service's persisted datastore.

- Be mindful about error handling. It is up to you to choose a solution that feels right.

- Avoid duplication and extract re-usable modules where it makes sense.
  We want to see your approach to creating a codebase that is easy to maintain.

- Unit test one module of choice. There is no need to test the whole app as we only want to understand what you take into consideration when writing unit tests.

- You may add as many commits and branches as you like.

- You may add as many comments as you like but there is no need to overdo it.


## When you are done

1. Commit/merge your final solution to the master or main branch of the git repository.

2. Create a zip file with the project.

3. Send the result to us by the contact address.

If you have questions or problems, please dont hesitate to mail us!

## Contact address

jilhol@carnegie.se
