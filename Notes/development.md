1. Read the docs

2. Intial db structure from README.md

3. Imagine Test Cases

4. Build the code
    - Install .NET SDK 8.0.20

5. Mediator Pattern
- what is the request flow?
    - request -> controller -> mediator (preprocessor -> handler -> postprocessor) -> controller response
    - Example:
      1. `POST /AstronautDuty`
      2. `AstronautDutyController.CreateAstronautDuty`
      3. `Mediator.send`
      4. `(CreateAstronautDutyPreProcessor : IRequestPreProcessor<CreateAstronautDuty>).CreateAstronautDutyPreProcessor.Process`
      5. `CreateAstronautDutyHandler : IRequestHandler<CreateAstronautDuty, CreateAstronautDutyResult>.Handle`
      6. `CreateAstronautDutyPostProcessor: IRequestPostProcessor<CreateAstronautDuty, CreateAstronautDutyResult>.Process`
      7. `AstronautDutyController.CreateAstronautDuty.GetResponse(result)`

- What should belong in each partition of the mediator (preprocessor -> handler -> postprocessor)?
  - preprocessor
  - handler
  - postprocessor

6. Run the migrations and examine the initial table structure
- Install SQLite/SQL Server Compact Toolbox Extension 
- Examine the InitialCreate migration
- Create enum tables for duty rank and title

7. Add logging

8. Manual testing and bugfixing
- Basic functionality:
  - Retrieve a person by name.
    - ` GET /Person/{name}`
  - Retrieve all people.
    - `GET /Person`
  - Add/update a person by name.
    - What is a person other than name? Associated astronaut records?
    - `PUT /Person`
  - Retrieve Astronaut Duty by name.
    - `GET /AstronautDuty/{name}`
  - Add an Astronaut Duty.
    - `POST /AstronautDuty`
9. Unit tests
  - XUnit?

10. UI
- Angular
- Angular Material Component Library