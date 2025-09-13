## TODO

### Add/bugfix remaining endpoints
- `PUT /person/{name}` is missing
    - what updating a person mean?

- add `GET /Career` to retrieve a person's AstronautDetails

- `GET /AstronautDuty` should return a person's current duty.. if one exists instead of their astronaut details

- `GET /person` and `GET /person/{name}` just returns a list the string names without their employment history

- `POST /astronautDuty` needs date checking and to update person's careerDates and the most recent duty's enddate

### Add logging
- add logging everywhere

- store log information

### Test coverage



## DONE
- `POST /person` doesn't enforce unique names (gonna keep names as PK's cuz it's in the rules)

- Asronaut dutyTitles and ranks should come from an enum table
    - need to add a migration for this
    - `POST /AstronautDuty` should handle enum values and not-founds