# OrderManagement - Sergiu

This program can take a list of items and clients from a binary tree, and manage speciffic orders based on the items stock, you can easily make an item at a discounted price by right clicking it and pressing the edit button, see a list of past orders or check again if the current order has everything you need.

##Features

-Adding items and clients to a list
-you can search on a list of every item from the inventory, or you can use the manual search function
-You can edit every aspect from an added item or remove it completly
-you can edit the name of a client
-Add any of the items from the inventory to the order, if you add the same item twice it will update the quantity in the shopping cart, also you can adjust the quantity of every item from the shopping cart, if the quantity is 0 the item is removed from the shopping cart, also if the quantity exceeds the available stock the program prints an error message.
-you can check the orders history of every client, or if you make another order for a client the order history is updated with the coresponding quantities
-the program uses binary trees for efficiency in searching for a speciffic object, also an object pool is used instead of instantiating and destroying the inventory UI elements.

##Usage
-with this program you can easily manage orders from any store, you can add all of your product and client information in the program and easily keep track of client orders, quantities and the remaining stock, you can edit an item information or make a sale with a discounted price.

##Roadmap
Future improvements to the program include:
-an updated UI
-more code optimizations
-make the order history separated by orders not one per client with new items added when making another order
-posibility to print the orders history to an .pdf file

##License
Copyright (c) 2019 Sergiu Pop

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
