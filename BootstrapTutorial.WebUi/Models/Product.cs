using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BootstrapTutorial.WebUi.Models
{
    public class Product
    {
        public int? Id { get; set; }

        // Regex that allows:                   // ^                   # Start of the string
        // 1. Only alphabetic characters        // (?![\s])            # Negative lookahead - ensures no leading whitespace
        // 2. Only single spaces between words  // [a-zA-Z]+           # One or more alphabetic characters
        // 3. No leading or trailing spaces     // (?:\s[a-zA-Z]+)*    # Non-capturing group for optional space-separated words
        // 4. No multiple consecutive spaces    // (?<!['\s])$         # Negative lookbehind - ensures no trailing whitespace or apostrophe
        [RegularExpression(@"^(?![\s])[a-zA-Z]+(?:\s[a-zA-Z]+)*(?<!['\s])$",
        ErrorMessage = "Invalid input. Use only alphabetic characters with single spaces between words.")]
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal? Price { get; set; }

        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters.")]
        public string? Description { get; set; }
    }
}
