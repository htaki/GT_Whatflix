Whatflix:
- Framework: ASP.NET Core 2.1
- Databases: ElasticSearch 6.0 and MongoDB 4
- Deployment: Docker and Heroku

Overview:
- The application has the following layers:
	1. Presentation (Endpoints)
	2. Domain 
	3. Data (Mongo/ElasticSearch repositories)
	4. Infrastructure (Common methods, Injections)

Api Endpoints:
	- Has the endpoint for searching the user by userId and searchText. 
	- And also, the endpoint fetching the user's movie recommendations.
	- Endpoints for add, post, update, delete ElasticSearch Index.
	- Endpoint for replicating the Mongo/ElasticSearch database.

The repository (Mongo/ElasticSearch) is chosen during runtime. 
	- To use mongo repository set 'UseMongoRepository' to true in appsettings.json.
	- To use Elasticsearch repository set 'UseMongoRepository' to false in appsettings.json.

Assumptions / Logic:
	1. User preference search matches -> (any preferred_languages AND (any favourite_actors OR any favourite_directors))
	2. Search words is a non-empty string array.
	3. For recommendations: A field called 'AppearedInSearchs' is added in movies collection which is incremented each time the movie is appeared in the user's recommended search.
		This adds to the relevance of the search.

Application Url:
http://whatflix.herokuapp.com

Swagger Url:
http://whatflix.herokuapp.com

Deployment:
	1. Please find the script (./deploy.sh) to automatically deploy to Heroku. (Requires Docker and Heroku CLI.)
	2. The script also executes the unit tests.

Document schema:
{
    "_id" : 14139,
    "Title" : "Timecrimes",
    "Language" : "Spanish",
    "Director" : "Nacho Vigalondo",
    "Actors" : [ 
        "Karra Elejalde", 
        "Candela Fernández", 
        "Bárbara Goenaga", 
        "Nacho Vigalondo", 
        "Juan Inciarte", 
        "Libby Brien", 
        "Nicole Dionne", 
        "Philip Hersh"
    ],
    "AppearedInSearches" : 0
}