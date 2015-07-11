// CppCliTestApp.cpp : main project file.

#include "stdafx.h"
#include <exception>
#include <iostream>

using namespace System;
using namespace Observer;

static int counter = 0;

template<class T> void AssertAreEqual(T a, T b, int line)
{
	if (a != b)
	{
		std::cout << "ASSERTION FAILED @ line: " << line;
	}
}

void myCallback(Object^ obj, Boolean)
{
	counter++;
}

int main(array<System::String ^> ^args)
{
	MyViewModel^ ViewModel = gcnew MyViewModel();

	//Subscribe to property in C++/CLI: create Action (delegate) matching the property's type (Boolean)
	ViewModel->MyLocalProperty->Subscribe(gcnew Action<Object^, Boolean>(myCallback));
	
	AssertAreEqual<int>(counter, 0, __LINE__);

	ViewModel->MyLocalProperty->Value = !ViewModel->MyLocalProperty->Value;

	AssertAreEqual<int>(counter, 1, __LINE__);

	std::cin.get();

    return 0;
}



