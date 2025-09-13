## TODO

### Add/bugfix remaining endpoints
- `PUT /person/{name}` is missing
    - what updating a person this mean?

- `POST /astronautDuty` needs date checking and to update person's careerDates and the most recent duty's enddate

- swap insane queries for LINQ

### Add logging
- add logging everywhere

- store log information

### Test coverage



## DONE
- `POST /person` doesn't enforce unique names (gonna keep names as PK's cuz it's in the rules)

- Asronaut dutyTitles and ranks should come from an enum table
    - need to add a migration for this
    - `POST /AstronautDuty` should handle enum values and not-founds

- add `GET /Career` to retrieve a person's AstronautDetails
    - hydrated results for person calls

- `GET /AstronautDuty` should return a person's current duty.. if one exists instead of their astronaut details
    - misconception. AstronautDetails is correct and the collections live in Person objects

- `GET /person` and `GET /person/{name}` just returns a list the string names without their employment history