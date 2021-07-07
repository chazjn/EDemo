# EDemo
### Appointment Booking Api Demo Project

The project can be started via docker-compose. 
When started the following services will be available:

- http://localhost:5000 - gateway service that offers the public appointment api endpoints
- http://localhost:5001 - gateway service that offers the internal apointment api endpoints

In addition the following services are also exposed for debugging purposes:
- http://localhost:5002 - appointment api endpoints
- localhost:5003 - sql server


### Key Logic:

The `AppointmentValidator` object performs the basic business logic on the appointment creation/change/cancelation date/time requests.
The `ValidatorFactory` constructor takes a 'IAppointmentParameters' type. 
`StandardAppointmentParameters` is currently the only class that has this interace, and gets injected in startup.cs. 
This contains the values of the appoitment as per the given spec.
My thoughts for extracting these values out of the core business logic is that potentially different patients types could have different limits. 
E.g. some patients could be able to cancel appointments up-to one day before or book appointments four weeks aheads. 
When the appointment request is made, the parametefs for the patient could be read from a database.

The ValidatorFactory returns an IAppointmentValidator<T> depending on the type of appoitnment request. 
The DateTime that validation is performed is set in the constructor. Although this property is internal set, 
it can be set by the UnitTests project so it is possible to perform consistant unit tests.

After the appointment date/time is validated, the controller will attempt to perform the requested action. 
This involves interacting with both the Repo and EquipmentAvailabilityService.
I think here a further abstraction would be an advantage and I should move these two systems into a class such as an 'AppointmentBookingService', 
instead of this logic being inside the constructor.

Essentially the equipment/appointment logic is as follows:

Create appointment:
1. Attempt to reserve equipment
2. If no equipment available then return
3. Attempt to create appointment
4. If cannot create appoitment then unreserve equipment

Change appointment:
1. Attempt to reserve equipment
2. If no equipment available then return
3. Attempt to change appointment
4. If cannot change appointment then unreserve equipment

Cancel appointment:
1. Attempt to cancel appointment
2. If cancellation successful then unreserve equipment

My main issue with this logic is that what happens if there is an exception in between reserving/unreserving equipment and creating/changing/canceling the appointment. 
Obiously we could have some exception handling to tidy up but still the equipment/appointments are two seperate systems - 
so there is a possibility that these two systems would go out of sync. 
Would we need some sort of reconcillation service that would periodically check and keep these systems in sync with each other? 
I thought about another layer of 'confirmation' that both the equipment/appointment systems use, however it reminded me of the two generals problem: 
https://en.wikipedia.org/wiki/Two_Generals%27_Problem

### Improvements

 - I note that when starting the project the appointment api fails to run first time because it is attempting to interact with the sql server before it is ready.
  I looked up a few techniques to delay the api project starting until the sql server had finished loading, however I have not tried any yet.
  Instead for the time-being I have added the following line in the docker-compose file: `restart: always`. Now the api project automatically attempts to re-load if it crashes on startup.
  
- sql sa password to be stored in secrets.json

- Separate sql username/password to be set for the appointment api with the least privideges required

- sql server to use docker volume for permanent storage

- Exception handling and logging in controller. Not sure what the best practice here is regarding microservices, 
I could wrap each method with a `try/catch` block but this doesn't seem like the best way.

- Create a `AppointmentBooking` class that both the `AppointmentsRepository` and the `EquipmentAvailababilityService` are in injected into. 
I feel there is still too much logic happening in the controller methods and this should be abstracted away.

- Perhaps the appointment api could have followed more of a RESTful standard regarding changing/canceling appointments. 
Instead of searching by PatientId and DateTime, it would be a cleaner solution to make the request by the AppoitmentId.
E.g. `DELETE /appointments/1/` for cancel or `PUT /appointments/1/` for making a change to the appoitment.

- Adding dynamic versioning as part of route in Ocelot gateways

- Rather than building the `Email` object in the controller and sending the email this task could be handed over to the `EmailNotificationSystem`. 
The controller could select the appropriate email template and pass in the required data points (name, email, appointment time etc.).

- Multiple timezones; I did consider this early on in the project and tried out a few different techniques but in the end decided to get the core project work done without it.
I looked at assigning each equipment item an IANA timezone and calculating and storing all appointment datetime values in UTC. 
However - I could not decide on a reliable way for the user to specify what timezone they were making the appoitment request from. 
I think this value would need to be stored server side and the user would specify in a settings page; 
it would then be up to the user to keep this setting up-to-date if they travelled into a different timezone.

- I note it is possible to 'game' the system if you wanted to cancel an appointment but were within 2 days of an appointment (instead of 3 days).
You could change you appointment to a later date and then immediately cancel it; this issue could be overcome by an apppointment 'ammendment' limiter, 
e.g. only one change per 24 hours without additional authorisaion. 

- Authentication; I did not look at this for this project but obviously this type of service would require authentication.
