Project keywords: C#, WPF, Drag and Drop, HTML-editor

ABOUT THE APPLICATION

A Windows Presentation Foundation-based desktop application that helps the user create HTML and CSS files. The user can use the radio buttons to choose whether the program generates HTML or CSS boilerplate code. The application supports C# drag and drop and provides ready-made HTML or CSS tags in Button elements. These tags can be dragged and dropped into a TextBox element where the HTML page is written. Button elements contain either HTML or CSS tags depending on which the user has selected. Users can also create their own HTML elements by typing them into the TextField
input field. After typing, the element is generated in its own button element and can be dragged and dropped into the TextBox element.

AUTOCOMPLETE TEXT

The user can also utilize automatic text completion when writing html tags. This feature can be enabled by clicking the checkbox.
When the feature is selected, the application's UI displays an autocomplete box element to the user. The autocomplete functionality
is implemented using the DotNetProjects.WpfToolkit.Input library. (https://www.nuget.org/packages/DotNetProjects.WpfToolkit.Input/).
The suggested HTML tags are loaded from a text file used by the application based on the user's input.

Example image of the auto-fill feature in use
![alt text](HtmlEditor/images/wpfAC.png)

RESPONSIVE FEATURES

Both the editing and preview windows can be resized with the click of a button. Each click changes the width and height of the window by 5 pixels.

SAVE AND LOAD FILE

The application uses C#'s FileDialog and File classes to save and load files.

WEBSITE PREVIEW

User can preview the website that is under construction. The application has a C# WebBrowser embedded in the same user interface. See sample image below.

![alt text](htmleditor.png)

