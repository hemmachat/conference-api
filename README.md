# Conference API
This is an proxy to invoke the conference API to fetch conference data. I have made the default result to show only 10 items to save waiting time. The requirements are not very clear, so I had to make some assumptions.

## Some Observation
- The filter of speaker, date and timeslot does not seem to work

## Code Features
- Cache some data
- There are some unit tests and some integration tests
- Use Swagger UI

## Some Future Improvements
- Result pagination
- Result limit the number of rows 
- Have more tests especially negative tests
- Need to check and handle errors