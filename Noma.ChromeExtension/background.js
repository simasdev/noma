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
  console.log("This will be added to Noma application " + selectedText);
}