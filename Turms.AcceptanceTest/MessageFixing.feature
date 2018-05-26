Feature: MessageFixing
	In order to process invalid messages
	As an interface engineer
	I want to fix errors in the messages

@mytag
Scenario: Parsing HL7 messages with invalid line breaks succeeds after fixing the message
	Given I have a message with a line break in the middle of a segment
	When I fix the message
	Then the fixed message can be parsed

@mytag
Scenario: Parsing HL7 messages with invalid line breaks without fixing fails
	Given I have a message with a line break in the middle of a segment
	Then the fixed message cannot be parsed
