Feature: MessageProcessing
	In order to process different messages
	As an interface engineer
	I want to be able to register message processors

@mytag
Scenario: Trigger appropriate processor
	Given I have an ADT^A01 message processor
	And I have an ADT^A02 message processor
	And I have an ADT^A08 message processor
	When I process an ADT^A01 message
	Then the ADT^A01 message processor is triggered
	And the ADT^A02 message processor is not triggered
	And the ADT^A08 message processor is not triggered
