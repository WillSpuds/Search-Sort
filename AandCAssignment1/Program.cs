using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;

namespace AandCAssignment1
{
    class Program
    {
        //Create a constant integer to be used when values searched for cannot be found
        const int NotFound = -1;
        static void Main(string[] args)
        {
            //File selection variables
            int fileNumber = 0;
            int chosenFileInt = 0;
            int firstChosenFileInt = 0;
            int secondChosenFileInt = 0;

            //Array management variables
            int[] dataArrayInt = null;
            string[] dataArrayString;

            //Timer and counter variables
            int counter = 0;
            long elapsedMs = 0;
            float timeElapsed = 0;

            


            //Get the current directory
            string currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            //Create path to data directory
            string dataFolder = Path.Combine(currentDirectory, "Data");

            string[] filePaths = Directory.GetFiles(dataFolder, "*.txt");

            //Display each of the files
            foreach (var fileName in filePaths)
            {
                fileNumber++;
                Console.WriteLine(fileNumber + ": " + Path.GetFileName(fileName));
            }

            //Give the user an option to merge after all files are displayed
            int mergeFileNumber = fileNumber+1;
            Console.WriteLine(mergeFileNumber + ": Merge two files");


            Console.WriteLine("Please enter the number of the desired file");

            chosenFileInt = getIntFromInput(mergeFileNumber);

            if(chosenFileInt == mergeFileNumber)
            {
                //The user has selected to merge two files, first get both the indexs of desired files
                
                Console.WriteLine("Please enter the number of the first file");
                firstChosenFileInt = getIntFromInput(fileNumber);

                Console.WriteLine("Please enter the number of the second file");
                secondChosenFileInt = getIntFromInput(fileNumber);

                string firstChosenFilePath = filePaths[firstChosenFileInt - 1];
                string[] firstDataArrayString = File.ReadAllLines(firstChosenFilePath);
                

                string secondChosenFilePath = filePaths[secondChosenFileInt - 1];
                string[] secondDataArrayString = File.ReadAllLines(secondChosenFilePath);

                //MERGE
                var tempList = new List<string>();
                tempList.AddRange(firstDataArrayString);
                tempList.AddRange(secondDataArrayString);

                //ToArray converts the new list into an array
                dataArrayString = tempList.ToArray();
            }
            else
            {
                string chosenFilePath = filePaths[chosenFileInt - 1];
                //read selected file into an array
                dataArrayString = File.ReadAllLines(chosenFilePath);
            }

            
            //Check that all elements within the file are integers
            try
            {
                dataArrayInt = Array.ConvertAll<string, int>(dataArrayString, int.Parse);
            }
            catch
            {
                Console.WriteLine("There was an error with the file. Check file and rerun the app.");
                System.Environment.Exit(-1);
            }

            Console.Clear();

            //Display Menu
            Console.WriteLine("Please enter the number of the desired algorithm");
            Console.WriteLine("1: Bubble Sort");
            Console.WriteLine("2: Insertion Sort");
            Console.WriteLine("3: Quick Sort");
            Console.WriteLine("4: Merge Sort");
            Console.WriteLine("");
            Console.WriteLine("5: Binary Search");
            Console.WriteLine("6: Interpolation Search");

            int chosenAlgoAsInt = getIntFromInput(6);
            Console.Clear();
            

            if (chosenAlgoAsInt < 5)
            {
                //Selected a sort

                //Ascending or Decending
                Console.WriteLine("Please enter the number of the desired sort order");
                Console.WriteLine("1: Ascending");
                Console.WriteLine("2: Descending");


                int chosenSortOrderAsInt = getIntFromInput(2);

                Console.Clear();

                //Get the step count
                Console.WriteLine("Please enter step number to display (1-256, 1 displays everything)");
                int chosenStepAsInt = getIntFromInput(256);

                bool ascending = false;

                if (chosenSortOrderAsInt == 1)
                {
                    ascending = true;
                }


                switch (chosenAlgoAsInt)
                {
                    //Switch statement to manage the different sorts

                    case 1:
                        //Create the stopwatch which will time all algorithms
                        var watch = Stopwatch.StartNew();
                        counter = BubbleSort(dataArrayInt, ascending);
                        watch.Stop();
                        timeElapsed = watch.ElapsedTicks;
                        break;

                    case 2:
                        watch = Stopwatch.StartNew();
                        counter = InsertionSort(dataArrayInt, ascending);
                        watch.Stop();
                        timeElapsed = watch.ElapsedTicks;
                        break;

                    case 3:
                        watch = Stopwatch.StartNew();
                        counter = QuickSort(dataArrayInt, ascending);
                        watch.Stop();
                        timeElapsed = watch.ElapsedTicks;
                        break;

                    case 4:
                        watch = Stopwatch.StartNew();
                        counter = MergeSort(dataArrayInt, ascending);
                        watch.Stop();
                        timeElapsed = watch.ElapsedTicks;
                        break;

                    default:
                        counter = BubbleSort(dataArrayInt, ascending);
                        break;
                }

                Console.Clear();
                Console.WriteLine("Here are your results: " );


                //Display results, only show values in the desired stepcount
                foreach (int i in dataArrayInt)
                {
                    if (i % chosenStepAsInt == 0)
                    {
                        Console.WriteLine(i);
                    }
                }
                

                //Display evaluation
                Console.WriteLine("This took "+ counter + " iterations");
                Console.WriteLine("in " + timeElapsed/10000 + " milliseconds");

            }
            else
            {
                //Search selected

                Console.WriteLine("Please enter number to search for:");
                int numberToSearchForAsInt = getIntFromInput(Int32.MaxValue);

                //Bubblesort the array to get into ascending order
                BubbleSort(dataArrayInt, true);

                int positionInArray = -1;
                int closestMatch = -1;

                int closestMatchPos = -1;

                switch (chosenAlgoAsInt)
                {
                    //Another switch case for the searches

                    case 5:
                        var watch = Stopwatch.StartNew();
                        positionInArray = BinarySearch(dataArrayInt, 0, dataArrayInt.Length, numberToSearchForAsInt, ref closestMatch, ref closestMatchPos);
                        watch.Stop();
                        timeElapsed = watch.ElapsedTicks;
                        break;

                    case 6:
                        watch = Stopwatch.StartNew();
                        positionInArray = InterpolationSearch(dataArrayInt, 0, numberToSearchForAsInt, ref closestMatch, ref closestMatchPos);
                        watch.Stop();
                        timeElapsed = watch.ElapsedTicks;
                        break;

                    default:
                        positionInArray = BinarySearch(dataArrayInt, 0, dataArrayInt.Length, numberToSearchForAsInt, ref closestMatch, ref closestMatchPos);
                        break;
                }

                Console.Clear();

                //Display results
                if (positionInArray > -1)
                {
                    Console.WriteLine("If the numbers are sorted in ascended order " + numberToSearchForAsInt + " is at position: " + positionInArray);
                }
                else
                {
                    Console.WriteLine("No match found, the closest match was "+ closestMatch + " at position: " + closestMatchPos);
                }

                Console.WriteLine("This search took " + timeElapsed / 10000 + " milliseconds");
            }
        }

        static int BubbleSort(int[] arrayToSort, bool ascending)
        {
            int loopCount = 0;
            int n = arrayToSort.Length;

            for (int i = 0; i < n - 1; i++)
            {
                loopCount++;
                for (int j = 0; j < n - 1 - i; j++)
                {
                    loopCount++;
                    
                    if(ascending)
                    {
                        if (arrayToSort[j + 1] < arrayToSort[j])
                        {
                            int temp = arrayToSort[j];
                            arrayToSort[j] = arrayToSort[j + 1];
                            arrayToSort[j + 1] = temp;
                        }
                    }
                    else
                    {
                        if (arrayToSort[j + 1] > arrayToSort[j])
                        {
                            int temp = arrayToSort[j];
                            arrayToSort[j] = arrayToSort[j + 1];
                            arrayToSort[j + 1] = temp;
                        }
                    }

                    
                }
            }
            return loopCount;
        }

        static int InsertionSort(int[] arrayToSort, bool ascending)
        {
            int loopCount = 0;
            int n = arrayToSort.Length;

            int numSorted = 1; 
            int index; 
            while (numSorted < n)
            {
                loopCount++;
                int temp = arrayToSort[numSorted];
                
                for (index = numSorted; index > 0; index--)
                {
                    loopCount++;

                    if(ascending)
                    {
                        if (temp < arrayToSort[index - 1])
                        {
                            arrayToSort[index] = arrayToSort[index - 1];
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (temp > arrayToSort[index - 1])
                        {
                            arrayToSort[index] = arrayToSort[index - 1];
                        }
                        else
                        {
                            break;
                        }
                    } 
                }
                arrayToSort[index] = temp;
                numSorted++;
            }
            return loopCount;
        }
        
        static int QuickSort(int[] arrayToSort, bool ascending)
        {
            return QuickSortMain(arrayToSort, 0, arrayToSort.Length - 1, ascending);           
        }
        static int QuickSortMain(int[] arrayToSort, int left, int right, bool ascending)
        {
            int i, j;
            int pivot, temp;
            int loopCount = 0;
            var internalLoop = 0;

            i = left;
            j = right;
            pivot = arrayToSort[(left + right) / 2];

            do
            {
                if(ascending)
                {
                    while ((arrayToSort[i] < pivot) && (i < right))
                    {
                        i++;
                        loopCount++;
                    }
                    while ((pivot < arrayToSort[j]) && (j > left))
                    {
                        j--;
                        loopCount++;
                    }
                }
                else
                {
                    while ((arrayToSort[i] > pivot) && (i < right))
                    {
                        i++;
                        loopCount++;
                    }
                    while ((pivot > arrayToSort[j]) && (j > left))
                    {
                        j--;
                        loopCount++;
                    }
                }
                

                if (i <= j)
                {
                    temp = arrayToSort[i];
                    arrayToSort[i] = arrayToSort[j];
                    arrayToSort[j] = temp;
                    i++;
                    j--;
                }

            }
            while (i <= j);

            loopCount++;

            if (left < j)
            {
                internalLoop = QuickSortMain(arrayToSort, left, j, ascending);
            }

            if (i < right)
            {
                internalLoop = QuickSortMain(arrayToSort, i, right, ascending);
            }

            loopCount = loopCount + internalLoop;
            return loopCount;

        }
        
        static int MergeSort(int[] arrayToSort, bool ascending)
        {
            int n = arrayToSort.Length;
            int[] tempArray = new int[n];
            return MergeSortMain(arrayToSort, tempArray, 0, n - 1, ascending);
        }
        static int MergeSortMain(int[] arrayToSort, int[] temp, int low, int high, bool ascending)
        {
            int n = high - low + 1;
            int middle = low + n / 2;
            int i;

            int loopCount = 0;
            var internalLoop = 0;

            if (n < 2)
            {
                return loopCount;
            }

            for(i = low; i<middle; i++)
            {
                temp[i] = arrayToSort[i];
                loopCount++;
            }

            //Sort bottom half
            internalLoop = MergeSortMain(temp, arrayToSort, low, middle - 1, ascending);
            loopCount = loopCount + internalLoop;

            //Sort top half
            internalLoop = MergeSortMain(arrayToSort, temp, middle, high, ascending);
            loopCount = loopCount + internalLoop;

            //Bring them back together
            internalLoop = Merge(arrayToSort, temp, low, middle, high, ascending);
            loopCount = loopCount + internalLoop;

            return loopCount;
        }
        static int Merge(int[] arrayToSort, int[] temp, int low, int middle, int high, bool ascending)
        {
            int loopCount = 0;

            int resultIndex = low;
            int tempIndex = low;
            int destinationIndex = middle;

            while (tempIndex < middle && destinationIndex <= high)
            {
                loopCount++;
                if(ascending)
                {
                    if (arrayToSort[destinationIndex] < temp[tempIndex])
                    {
                        arrayToSort[resultIndex++] = arrayToSort[destinationIndex++];
                    }
                    else
                    {
                        arrayToSort[resultIndex++] = temp[tempIndex++];
                    }
                }
                else
                {
                    if (arrayToSort[destinationIndex] > temp[tempIndex])
                    {
                        arrayToSort[resultIndex++] = arrayToSort[destinationIndex++];
                    }
                    else
                    {
                        arrayToSort[resultIndex++] = temp[tempIndex++];
                    }
                }
                
            }

            while (tempIndex < middle)
            {
                loopCount++;
                arrayToSort[resultIndex++] = temp[tempIndex++];
            }

            return loopCount;
        }


        static int BinarySearch(int[] arrayToSearch, int start, int stop, int numToSearchFor, ref int closestMatch, ref int closestMatchPos)
        {
            int mid = (start + stop) / 2;           

            if (start > stop)
            {
                return NotFound;
            }      

            if (numToSearchFor == arrayToSearch[mid])
            {
                return mid;
            }   
            else if (numToSearchFor < arrayToSearch[mid])
            {
                closestMatch = arrayToSearch[mid-1];
                closestMatchPos = mid - 1;
                return BinarySearch(arrayToSearch, start, mid - 1, numToSearchFor, ref closestMatch, ref closestMatchPos);
            }
            else
            {
                closestMatch = arrayToSearch[mid + 1];
                closestMatchPos = mid + 1;
                return BinarySearch(arrayToSearch, mid + 1, stop, numToSearchFor, ref closestMatch, ref closestMatchPos);
            }
                
        }
        static int InterpolationSearch(int[] arrayToSearch, int i, int numToSearchFor, ref int closestMatch, ref int closestMatchPos)
        {
            int low, high, denominator;
            int mid = 0;
            i = 0;

            low = 0; 
            high = arrayToSearch.Length - 1; 
            
            if ((arrayToSearch[low] <= numToSearchFor) || (numToSearchFor <= arrayToSearch[high]))
            {
                while (low <= high && i == 0)
                {
                    Console.WriteLine(high);
                    Console.WriteLine(low);

                    Console.WriteLine(arrayToSearch[high]);
                    Console.WriteLine(arrayToSearch[low]);

                    denominator = arrayToSearch[high] - arrayToSearch[low];
                    if (denominator == 0)
                    {
                        mid = low;
                    }
                    else
                    {
                        mid = low + (((numToSearchFor - arrayToSearch[low]) * (high - low)) / denominator);
                    }

                    if (numToSearchFor == arrayToSearch[mid])
                    {
                        i = mid;
                    }
                    else if (numToSearchFor < arrayToSearch[mid])
                    {
                        high = mid - 1;
                    }
                    else
                    {
                        low = mid + 1;
                    }

                }
                
            }

            if (mid == numToSearchFor)
            {
                return mid;
            }
            else
            {
                //ask about position
                closestMatch = arrayToSearch[mid];
                closestMatchPos = mid;
                return NotFound;
            }
        }


        static int getIntFromInput(int max)
        {
            //Method for error checking all inputs within the applciation

            int returnInt = 0;
            
            bool validInput = false;
            
            while (validInput == false)
            {

                string inputString = Console.ReadLine();
                
                //Makes sure that the inputted value is an integer AND within range, which is passed in as a parameter
                try
                {
                    returnInt = Convert.ToInt32(inputString);
                    if (returnInt >= 1 && returnInt <= max)
                    {
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a number between 1 and " + max);
                    }
                }
                catch
                {
                    Console.WriteLine("Only enter a number");
                }             
            }
            return returnInt;
        }
    }
}
