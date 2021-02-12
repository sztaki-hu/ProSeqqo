using System;

namespace SequencerConsole
{
    internal class ConsoleSpinner
    {
        private int _currentAnimationFrame;

        public ConsoleSpinner()
        {
            //SpinnerAnimationFrames = new[]
            //                         {
            //                             '|',
            //                             '/',
            //                             '-',
            //                             '\\'
            //                         };
            SpinnerAnimationFrames = new string[]
                                     {
                                         "[>............................] 2%",
                                         "[|>...........................] 4%",
                                         "[/=>..........................] 9%",
                                         "[-==>.........................]12%",
                                         "[\\===>........................]15%",
                                         "[|====>.......................]17%",
                                         "[/=====>......................]20%",
                                         "[-======>.....................]22%",
                                         "[\\=======>....................]28%",
                                         "[|========>...................]32%",
                                         "[/=========>..................]37%",
                                         "[-==========>.................]45%",
                                         "[\\===========>................]51%",
                                         "[|============>...............]59%",
                                         "[/=============>..............]60%",
                                         "[-==============>.............]62%",
                                         "[\\===============>............]68%",
                                         "[|================>...........]70%",
                                         "[/=================>..........]75%",
                                         "[-==================>.........]77%",
                                         "[\\===================>........]82%",
                                         "[|====================>.......]84%",
                                         "[/=====================>......]86%",
                                         "[-======================>.....]88%",
                                         "[\\=======================>....]90%",
                                         "[|========================>...]92%",
                                         "[/=========================>..]95%",
                                         "[-==========================>.]99%",
                                         "[\\===========================>]100%",
                                         "[|............................] 0%"
                                     };
        }

        public string[] SpinnerAnimationFrames { get; set; }

        public void UpdateProgress()
        {
            Console.CursorVisible = false;
            // Store the current position of the cursor
            var originalX = Console.CursorLeft;
            var originalY = Console.CursorTop;

            // Write the next frame (character) in the spinner animation
            Console.Write(SpinnerAnimationFrames[_currentAnimationFrame]);

            // Keep looping around all the animation frames
            _currentAnimationFrame++;
            if (_currentAnimationFrame == SpinnerAnimationFrames.Length)
            {
                _currentAnimationFrame = 0;
            }

            // Restore cursor to original position
            Console.SetCursorPosition(originalX, originalY);
        }
    }
}
