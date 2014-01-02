z-wave-microservice
===================

An implementation of the z-wave protocol for .NET as a micro service.

The application can be hosted through tcp in a server and accept commands that are been transmitted across the z-wave network.

I have been working/testing with the following hardware:
 - Controller	: Aeon Z-Stick
 - Devices		: Fibaro wall plug

 I am in the process of creating another service that transmits the messages from the z-wave network to custom micro services/http endpoints.

 Start by using the update.bat to download the micro-services host and update the tools. Use the z-wave tests project to test with your own hardware.
