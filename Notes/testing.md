### Rules & Test Cases
1. A Person is uniquely identified by their Name.
- Test1
    - POST/SEED person record
    - GET by name
    - ASSERT person match
- Test2
    - POST/SEED person record
    - GET missing name
    - Graceful 404

2. A Person who has not had an astronaut assignment will not have Astronaut records.
- Test1
    - POST person in people table
    - GET this person in astronaut table
    - ASSERT no record (404)

3. A Person will only ever hold one current Astronaut Duty Title, Start Date, and Rank at a time.
- Test1
    - POST/PUT new person to astronaut duty
    - ASSERT singular record in astronaut table

4. A Person's Current Duty will not have a Duty End Date.
- Test1
    - POST a person
    - GET person
    - ASSERT no enddate

5. A Person's Previous Duty End Date is set to the day before the New Astronaut Duty Start Date when a new Astronaut Duty is received for a Person.
- Test1
    - POST a new person
    - ASSERT no enddate
    - POST/PUT an associated Asstronaut record (future dated)
    - GET person enddate
    - GET astronaut duty startdate
    - ASSERT (person enddate = astronaut duty startdate - 1 day)

6. A Person is classified as 'Retired' when a Duty Title is 'RETIRED'.

7. A Person's Career End Date is one day before the Retired Duty Start Date.
- Test1
    - POST a new person
    - POST a RETIRED duty start date for this person
    - GET person's table row
    - GET person's astronaut duty row
    - ASSERT (Career End Date = Retired Duty Start Date - 1 day)