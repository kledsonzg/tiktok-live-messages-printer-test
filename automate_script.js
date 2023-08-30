function ExecuteScript() 
{ 
	let allElements = document.getElementsByTagName('div');
	let messageList = null;

	for(let i = 0; i < allElements.length; i++)
	{
		let className = allElements[i].getAttribute('class');
		if(className == null)
			continue;

		if(className.includes('DivChatMessageList') == false)
			continue;

		messageList = allElements[i];
		break;
	}
	
	const childs = messageList.getElementsByTagName('div');
	let result = '';
	for(let i = 0; i < childs.length; i++)
	{
		let className = childs[i].className;
		if(className.includes(' read_completed') || className.includes('DivChatMessageContent') == false)
			continue;
		let userName = '';
		let message = '';
		let messageChilds = childs[i].getElementsByTagName('span');

		for(let j = 0; j < messageChilds.length; j++)
		{
			let _type = messageChilds[j].getAttribute('data-e2e');
			if(_type == 'message-owner-name')
			{
				userName = messageChilds[j].innerHTML;
				continue;
			}
			else if(messageChilds[j].className.includes('SpanChatRoomComment') )
			{
				message = messageChilds[j].innerHTML;
				continue;
			}
		}

		childs[i].className += ' read_completed';
		result += userName + ': ' + message + '\n';
		continue;
	}

	result = result.substring(0, result.lastIndexOf('\n') );
	return result;
}