﻿Feature: MessageParsing
	In order to process a message
	As an interface engineer
	I want to parse a message string to an object structure

@mytag
Scenario: Parse ADT^A01 message
	Given I have an ADT^A01 message
	When I parse the message
	Then the patient ID can be extracted
	And  the patient name can be extracted

@mytag
Scenario: Parse a message with encoded characters
	Given I have a message with encoded characters
	When I parse the message
	Then the fields with encoded characters can be interpreted properly
	And the message serializes back to the original message
