Feature: MessageCreation
	In order to process invalid messages
	As an interface engineer
	I want to create new HL7 messages

@mytag
Scenario: Create a new ADT^A01 message
	Given I create a new message
	And I populate the MSH Segment
	And I populate the PID Segment
	When I encode the message
	Then the message can be parsed
