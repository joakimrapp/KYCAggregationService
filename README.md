# KYC Aggregation Service Task

As I've mentioned in our discussions I have not developed in .NET for many years now. It was really fun to "dust-off" the old skills and it's really a pleasure to see that it has evolved in the last 10 years.

I have tried to keep the code simple and easy to follow, but I'm sure that there are many things that could have been done in a better way. I'm looking forward to your feedback on this.

I have probably missed some to use techiques and patterns that are common in the .NET world today, but I hope that you can see that I have tried to keep the code clean and easy to follow.

I'm also certain that if working with .NET again I would quickly pick up the new patterns and techniques. I have always been a quick learner and I'm sure that I would be able to contribute to the team in a short time.

## General Coding Style

### Comments

I very rarley comment anything in my code, since I believe that the code should be self-explanatory and that comments in general have a tendency to create more confusion than clarity.

There are absolutely situations when comments are justified, for example to explain unintuative logic in the code due to business rules that are not obvious from the code itself.

I really recommed `Clean Code: A Handbook of Agile Software Craftsmanship` by Robert C. Martin, which I know many developers already have read. Especially the chapter about comments!

### Source Code

I'm not a big fan of scrolling in general and I have a general rule of trying to keep the source code of each file within the visible area of the editor.

This topic is, of course, a matter of opinon and I know that there are developers that prefer fewer files. Also my tendency to write long one-liners and the use of "side-effects" in the code is not to everybodys taste.

I'm not sure that this code really reflects any of this, but I thought I should mention it anyway.

### Logging

I have not focused on logging in this task. The reason for that is that the actual task was not that big and I do not expect the code to be maintained or used in anyway.

Having said that, I still belive that logging in general is a very important and quite complex topic.

If done in the "wrong way" it can not only make the code harder to read, maintain and debug, but also be rather expensive in terms of actual cost.

## Design

### Error Handling

If this was a real-world application I would have spent more time on the error handling. In this task I have focused on the happy path and not spent much time on the error handling.

I have fully "trusted" the CustomerDataService for example, which is not a good practice in a real-world application. This applies to both the quality of data it produces and the availability of the service.
I hope this explains the rather sloppy solution of interpreting all exceptions as 404, customer not found.

This would absolutely not be acceptable in a production solution but I hope you have some understanding in that most of my time was spent on catching up with .NET and the Azure Functions.

### Caching

I have implemented a simple in-memory cache that stores the responses from the Customer Data API. This solution is not very scalable and should be replaced with a proper distributed caching solution, like Redis or similar.

Since I have developed this as an Azure function were each instance is caching data in-mem with a time-to-live it could really be problematic if master data changes and differant instances have cached data at differant times.
Users could get different results depending on which instance they are routed to. I normally never use ttl caching in a distributed environments but instead try to use event-sourcing/pub-sub to invalidate/update cache entries if possible.

Caching is a very interesting topic in general, especially in server-less environments. I am in general a big fan of caching and generally keeping data as flat, close (preferably in-mem if it makes sense) and "ready-to-use" as possible.
At the same time as I think that good solutions never should serve stale data, that is more stale than the "eventually consistent"-priciples often used today allows.

## Configuration

In my implementation I have used the not so clean method of reading values defined as class members with hard-coded values.

This is not a good practice and should be replaced with a proper way of reading configuration values using a common way of managing configuration within the organization, often accessed through environment variables in runtime.

A possible solution would probably be to use Azure App Configuration and/or Azure Key Vault to store the configuration values (the last for secret values of course).

I looked into this here for example `https://learn.microsoft.com/en-us/azure/azure-app-configuration/concept-enable-rbac#authentication-with-token-credentials`, but decided not to spend any time on that since I suspect that you already have some libs that I normally would have used.

## Testing

In this assignment I just wrote a small test for one of the flattening functions. The two tests mainly focuses on a happy-path, so additional testing would be needed to cover edge-cases and error handling.

In general I like to focus more on integration tests than unit tests, since I think that the integration tests are more valuable in the long run.
I also think that, from a TDD perspective, it's much simpler to write integration tests before code, compared to unit-tests.
This is because the integration tests are more "high-level" and are more about the actual functionality of the system, rather than the implementation details.
Additionally they can be written in any language and preferrably by other people than the acctual developers implementing the functionallity, in a perfect world together with the product owner before implementation.

## Issues

### OpenAPI Code Generation

I spent quite some time trying to find a library that could generate stubs for an Azure Function based on an OpenAPI specification.

I found a few libraries that could generate code for a Web API, but none that could generate code for an Azure Function v4 .NET 8 of .NET 9 in an isolated worker model.

All the existing libraries I could find are based on the old .NET 5 and .NET 6 and are not compatible with the new isolated worker model (autorest, OpenAPI Generator, Swashbuckle to mention a few).
I'm sure that I have missed something and that there are libraries that can do this.

In the end I used the "Connected Services" feature and created a client to the service itself in order to at least have the schema models generated for the service.

If this was a real task and there was no viable library that does this I would probably have written a generic tool myself that generates the code based on the OpenAPI specification.
This is not a very complex task and I have done it before, although not for .NET and c#.

Writing core libraries and frameworks are sort of my favourite thing, I'm not sure why but parsing, generating code and similar tasks are just really fun to me.
I also love to dig deep into RFC:s in order to understand every detail of standards and protocols in order to implement them in the best possible and secure way.

## Other Remarks

* I guess there was a little type-o in the OpenAPI specification for the Customer Data API. The `#/components/schemas/ContactDetails/properties/address` property does not correspond to the actual response from the API. In the actual response the property is named `addresses`. I have updated the specification to reflect this.

* The Customer Data API was missing operationIds for the operations, I've added these.



## When you are done

1. Commit/merge your final solution to the master or main branch of the git repository.

2. Create a zip file with the project.

3. Send the result to us by the contact address.

If you have questions or problems, please dont hesitate to mail us!

## Contact address

jilhol@carnegie.se
