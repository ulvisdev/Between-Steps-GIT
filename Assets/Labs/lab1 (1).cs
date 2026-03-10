using UnityEngine;

/// UNITY BASICS WORKSHEET FOR P5 STUDENTS
/// Try uncommenting each section below and press Play!
/// Check the Console (Window > General > Console) to see Debug.Log() output

public class UnityBasicsWorksheet : MonoBehaviour
{
    // ===== SECTION 1: VARIABLES =====
    // Just like p5, you declare variables to store values
    
    int myNumber = 42;
    float myDecimal = 3.14f;
    string myText = "Hello Unity!";
    bool isAlive = true;
    Vector3 myPosition = new Vector3(1, 2, 3);
    
    
    void Start()
    {
        // Start() runs ONCE when the game begins
        // Uncomment the section you want to try!
        
        //TryVariables(); DONE
        // TryMath(); DONE
        // TryIfStatements(); DONE
        TryForLoop(); 
        // TryWhileLoop();
        // TryFunctions();
        // TryInput();
        // TryTransform();
    }

    void Update()
    {
        // Update() runs EVERY FRAME
        // Uncomment this to make something happen continuously:
        
        // TryInput();
    }


    // ===== SECTION 2: VARIABLES & PRINTING =====
    void TryVariables()
    {
        Debug.Log("My number: " + myNumber);
        Debug.Log("My text: " + myText);
        Debug.Log("Am I alive? " + isAlive);
        
        // EXERCISE: Create variables and print them
        // 1. Create a variable called 'age' with your age
        // 2. Create a variable called 'name' with your name
        // 3. Create a variable called 'score' with a number
        // 4. Print all three using Debug.Log()
    }


    // ===== SECTION 3: MATH =====
    void TryMath()
    {
        int a = 10;
        int b = 3;
        
        Debug.Log("10 + 3 = " + (a + b));
        Debug.Log("10 - 3 = " + (a - b));
        Debug.Log("10 * 3 = " + (a * b));
        Debug.Log("10 / 3 = " + (a / b));
        Debug.Log("10 % 3 = " + (a % b));  // Modulo (remainder)
        
        // Random numbers (like p5.random())
        Debug.Log("Random 0-1: " + Random.value);
        Debug.Log("Random 1-10: " + Random.Range(1, 10));
        
        // EXERCISE: Do the math and print results
        // 1. Create two variables: x = 20, y = 7
        // 2. Print: x + y, x - y, x * y, x / y, x % y
        // 3. Print a random number between 1 and 100
        // 4. Calculate area of a rectangle (width=5, height=3) and print it
    }


    // ===== SECTION 4: IF STATEMENTS =====
    void TryIfStatements()
    {
        int x = 5;
        
        if (x > 3)
        {
            Debug.Log("x is greater than 3");
        }
        else if (x == 3)
        {
            Debug.Log("x equals 3");
        }
        else
        {
            Debug.Log("x is less than 3");
        }
        
        // Comparison operators:
        // x == 5   (equal to)
        // x != 5   (not equal to)
        // x > 5    (greater than)
        // x < 5    (less than)
        // x >= 5   (greater or equal)
        // x <= 5   (less or equal)
        
        // Logical operators:
        // x > 3 && x < 10    (AND - both must be true)
        // x < 3 || x > 10    (OR - at least one must be true)
        // !isAlive           (NOT - opposite)
        
        // EXERCISE: Check conditions
        // 1. Create a variable 'score' = 85
        // 2. If score >= 90, print "A"
        //    If score >= 80, print "B"
        //    If score >= 70, print "C"
        //    Otherwise print "F"
        // 3. Create a variable 'isStudent' = true
        // 4. If isStudent is true AND score > 80, print "Good job!"
    }


    // ===== SECTION 5: FOR LOOP =====
    void TryForLoop()
    {
        // Repeat code a set number of times
        
        for (int i = 0; i < 5; i++)
        {
            Debug.Log("Loop iteration: " + i);
        }
        
        // Count backwards:
        // for (int i = 10; i > 0; i--) { }
        
        // Count by 2s:
        // for (int i = 0; i < 10; i += 2) { }
        
        for (int i = 1; i <= 10; i++)
        {
            Debug.Log(i);
        }

        for (int i = 10; i > 0; i--)
        {
            Debug.Log(i);
        }

        for (int i = 0; i < 10; i+=2)
        {
            Debug.Log(i);
        }

        for (int i = 0; i <= 5; i++)
        {
            Debug.Log("Hello!");
        }
        // EXERCISE: Write loops
        // 1. Print numbers 1 through 10
        // 2. Print numbers 10 down to 1
        // 3. Print even numbers only (0, 2, 4, 6, 8)
        // 4. Print "Hello!" 5 times
    }


    // ===== SECTION 6: WHILE LOOP =====
    void TryWhileLoop()
    {
        int count = 0;
        
        while (count < 5)
        {
            Debug.Log("Count: " + count);
            count++;  // Same as count = count + 1
        }
        
        // ++ means add 1
        // -- means subtract 1
        // += means add to (count += 2 adds 2)
        // -= means subtract from
        
        // EXERCISE: While loops
        // 1. Count from 0 to 20 using a while loop
        // 2. Count down from 10 to 0
        // 3. Create a loop that prints "Still going!" while a variable 'fuel' > 0
        //    Decrease fuel by 2 each loop
        // 4. What's the difference between for and while loops?
    }


    // ===== SECTION 7: FUNCTIONS =====
    // Functions let you organize code and reuse it
    
    void TryFunctions()
    {
        // Call the functions below:
        SayHello();
        SayHello();  // Call it again!
        
        int result = Add(5, 3);
        Debug.Log("5 + 3 = " + result);
        
        int doubled = Double(10);
        Debug.Log("10 doubled = " + doubled);
        
        // EXERCISE: Create and call functions
        // 1. Write a function called Greet that takes a name (string) and prints "Hello, [name]!"
        // 2. Write a function called Multiply that takes two numbers and returns their product
        // 3. Call Greet with your name
        // 4. Call Multiply with 6 and 7, and print the result
    }
    
    // Function with no return value
    void SayHello()
    {
        Debug.Log("Hello from a function!");
    }
    
    // Function that takes parameters and returns a value
    int Add(int num1, int num2)
    {
        return num1 + num2;
    }
    
    int Double(int x)
    {
        return x * 2;
    }
    


    // ===== SECTION 8: KEYBOARD INPUT =====
    void TryInput()
    {
        // Press W, A, S, D keys
        if (Input.GetKey(KeyCode.W))
        {
            Debug.Log("W pressed!");
        }
        
        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(0, 0.1f, 0);  // Move up
        }
        
        // GetKey = held down
        // GetKeyDown = pressed once
        // GetKeyUp = released
        
        // EXERCISE: Input handling
        // 1. Check if A key is pressed and print "A is down"
        // 2. Check if Space is pressed (use GetKeyDown) and print "Space pressed once!"
        // 3. Check if D is pressed and move right by 0.05f
        // 4. What's the difference between GetKey and GetKeyDown?
    }


    // ===== SECTION 9: TRANSFORM (Position, Rotation, Scale) =====
    void TryTransform()
    {
        // Get current position
        Vector3 currentPos = transform.position;
        Debug.Log("Current position: " + currentPos);
        
        // Move to a new position
        // transform.position = new Vector3(5, 2, 0);
        
        // Move by an amount
        // transform.Translate(1, 0, 0);  // Move right 1 unit
        
        // Rotate
        // transform.Rotate(0, 45, 0);  // Spin 45 degrees
        
        // Scale (size)
        // transform.localScale = new Vector3(2, 2, 2);  // Double size
        
        // EXERCISE: Transform the object
        // 1. Print the current position
        // 2. Move the object to position (0, 5, 0)
        // 3. Rotate it 90 degrees around the Y axis
        // 4. Scale it to be twice as big (2, 2, 2)
        // 5. What's the difference between position and Translate?
    }


    // ===== SECTION 10: ARRAYS =====
    void TryArrays()
    {
        // Store multiple values in a list
        int[] numbers = { 10, 20, 30, 40, 50 };
        
        Debug.Log("First number: " + numbers[0]);   // 10
        Debug.Log("Second number: " + numbers[1]);  // 20
        
        // Loop through array:
        // for (int i = 0; i < numbers.Length; i++)
        // {
        //     Debug.Log(numbers[i]);
        // }
        
        // EXERCISE: Arrays
        // 1. Create an array of 5 colors (as strings): "red", "blue", "green", "yellow", "purple"
        // 2. Print the first and last color
        // 3. Loop through all colors and print them
        // 4. Create an array of 4 scores: 85, 92, 78, 88
        // 5. Calculate the sum of all scores
    }


    // ===== BONUS: SWITCH STATEMENT =====
    void TrySwitch()
    {
        int choice = 2;
        
        switch (choice)
        {
            case 1:
                Debug.Log("You chose 1");
                break;
            case 2:
                Debug.Log("You chose 2");
                break;
            case 3:
                Debug.Log("You chose 3");
                break;
            default:
                Debug.Log("Invalid choice");
                break;
        }
        
        // EXERCISE: Switch statements
        // 1. Create a variable 'day' = 3
        // 2. Use a switch to print the day name:
        //    1 = Monday, 2 = Tuesday, 3 = Wednesday, etc.
        // 3. What happens if you forget the 'break;' statement?
        // 4. When would you use switch instead of if/else?
    }
}
