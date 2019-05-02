Whatflix:
- Framework: ASP.NET Core 2.1
- Databases: ElasticSearch 6.5.4 and MongoDB 4
- Deployment: Docker and Heroku

Overview:
- The application has the following layers:
	1. Presentation (Endpoints)
	2. Domain 
	3. Data (Mongo/ElasticSearch repositories)
	4. Infrastructure (Common methods, Injections)

Api Endpoints:
	- Endpoint for searching the user by userId and searchText. 
	- Endpoint fetching the user's movie recommendations.
	- Endpoint for replicating the Mongo/ElasticSearch database.

The repository (Mongo/ElasticSearch) is chosen during runtime. 
	- To use mongo repository set 'UseMongoRepository' to true in appsettings.json (appsettings.Development.json for local environment).
	- To use Elasticsearch repository set 'UseMongoRepository' to false in appsettings.json (appsettings.Development.json for local environment).

Assumptions / Logic:
	1. User preference search matches -> (any preferred_languages AND (any favourite_actors OR any favourite_directors))
	2. Search words is a non-empty string array.
	3. For recommendations: A field called 'AppearedInSearchs' is added in movies collection which is incremented each time the movie is appeared in the user's preferred movie search.
		This adds to the relevance of recommendation search.
	4. No partial matches.
	5. User preference is stored in memory (read from user_preferences.json).

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