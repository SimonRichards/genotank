#include "stdafx.h"
#include "..\gp\operators.h"

using namespace System;
using namespace System::Text;
using namespace System::Collections::Generic;
using namespace	Microsoft::VisualStudio::TestTools::UnitTesting;
using namespace gp;

namespace gptest {
	[TestClass]
	public ref class operators {
	public: 

		[TestMethod]
        void TestAdd() {
            Constant fourty_two(42);
            Constant one_over_two(0.5);
            Add a(&fourty_two, &one_over_two);
		}
	};
}
