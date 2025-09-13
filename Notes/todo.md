## TODO
- `POST /person` doesn't enforce unique names (gonna keep names as PK's cuz it's in the rules)

- `GET /person` and `GET /person/{name}` just returns a list the string names without their employment history

- `PUT /person/{name}` is missing

- Asronaut dutyTitles and ranks should come from an enum table
    - need to add a migration for this
    - `POST /AstronautDuty` should handle enum values and not-founds

- `GET /AstronautDuty` is probably intended to return a person's latest duty and probably doesn't do that 

- `POST /astronautDuty` needs date checking and to update person's careerDates and the most recent duty's enddate

- Astronaut Detail's role needs to be sorted out

- add logging everywhere

- store log information


## DONE
- `POST /person` doesn't enforce unique names (gonna keep names as PK's cuz it's in the rules)