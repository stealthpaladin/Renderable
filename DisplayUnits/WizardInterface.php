<?php

require_once(__DIR__ . '/../Render.php');
//require_once(__DIR__ . '/../DataObject.php');
//require_once(__DIR__ . '../../core/Service.php');

$ApproachDisplayUnit = array();
$ApproachDisplayUnit['User']['Browser'] = new renderable('ul');

class WizardInterface extends renderable
{
	public $InterfaceLayout;
	public 	$InterfaceHeader;
	public 		$InterfaceTitlebar;
	public 	$InterfaceContent;
	public 	$InterfaceFooter;
	public 		$CancelButton;
	public 		$BackButton;
	public 		$NextButton;
	public 		$FinishButton;
	
	WizardInterface()
	{
		$this->tag='ul'; 
		$this->classes=array('Wizard, Interface');
		$this->children[]			= $InterfaceLayout 	= new renderable('li','',	array('classes'=>'InterfaceLayout') );

		$InterfaceLayout->children[]	= $InterfaceHeader 	= new renderable('ul','',	array('classes'=>array('HeaderLayout','controls')));
		$InterfaceLayout->children[]	= $InterfaceContent	= new renderable('ul','',	array('classes'=>array('ContentLayout','controls')));
		$InterfaceLayout->children[]	= $InterfaceFooter	= new renderable('ul','',	array('classes'=>array('FooterLayout','controls')));

		$InterfaceHeader->children[]	= $InterfaceTitlebar	= new renderable('li','',	array('classes'=>array('Header','controls'),'content'=>'Complete action by following steps.'));

		$InterfaceFooter->children[]	= $CancelButton		= new renderable('li','',	array('classes'=>array('Cancel',	'DarkRed',		'Button'),'content'=>'Cancel'));
		$InterfaceFooter->children[]	= $BackButton		= new renderable('li','',	array('classes'=>array('Back',	'DarkGreen',	'Button'),'content'=>'Back'));
		$InterfaceFooter->children[]	= $NextButton		= new renderable('li','',	array('classes'=>array('Next',		'DarkGreen',	'Button'),'content'=>'Next'));
		$InterfaceFooter->children[]	= $FinishButton		= new renderable('li','',	array('classes'=>array('Finish',	'DarkBlue',		'Button'),'content'=>'Finish'));

		$FinishButton->attributes['data-intent']='Autoform Insert ACTION';
	}
}

$ApproachDisplayUnit['Publication']['NewWizard'] = new WizardInterface();

?>