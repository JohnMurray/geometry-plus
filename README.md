# Geometry Plus

__Geometry Plus__ is the product of a code-challenge and, in reality,
is probably the most useless AutoCAD plugin you will find. However, if
you find it useful/intriguing/etc. then feel free to use it however
you like. 

## Building It
You might have to update the references paths as they point to the local
DLLs found in the AutoCAD SDK for .NET (ObjectARX2013 to be exact). Once
the references have been updated, there should be no problem building the
solution.

## Loading it
Use the `netload` command in autocad and select the DLL resulting from your
build. This should initialize the application and show a new toolbar. And
that's all there is to loading it.

## So... What does this do?
Well, like I said earlier, it's not extremely useful. It currently has one
function and that is to draw three concentric circles. You can do this a
couple of different ways.

### From the Toolbar
The toolbar that is shown when the extension is loaded only has one button.
This button, when clicked, will show you a Windows form to choose the location
and colors of the circles. When you are done configuring, you can simply press
the `Create` button to generate the circles.

### Using the Command Directly
If you don't like toolbars, then you can directly run the command
`ConcentricCircle` and that will launch the same Windows form to create the circles.