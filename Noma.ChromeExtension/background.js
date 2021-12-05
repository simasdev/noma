chrome.runtime.onInstalled.addListener(function() {
  chrome.contextMenus.create({
    "id": "btnAddToNoma",
    "title": "Add to Noma",
    "contexts": ["selection"]
  });

  chrome.contextMenus.onClicked.addListener(
   OnButtonAddToNomaClick
  )

});



function OnButtonAddToNomaClick(info, tab)
{
  var selectedText = info.selectionText;

  fetch("http://localhost:4333/Noma/", {
    method: 'post',
    headers: {
      "Content-type": "application/json"
    },
    body: JSON.stringify({
      NoteText: selectedText
  })
  })
  .then(function (data) {
  
  })
  .catch(function (error) {
    console.log('Request failed: please ensure that Noma is running on your computer');
  });
}