﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameReviewApp.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Favorite Game")]
        public string FavoriteGame { get; set; }

        [Required]
        [Display(Name = "Favorite Genre")]
        public Genre FavoriteGenre { get; set; }

        [Required]
        [Display(Name = "Username")]
        [MaxLength(15)]
        public string UserName { get; set; }

    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }


    public class EditUserViewModel
    {
        public EditUserViewModel() { }

        public EditUserViewModel(ApplicationUser user)
        {
            this.UserName = user.UserName;
            this.FavoriteGame = user.FavoriteGame;
            this.FavoriteGenre = user.FavoriteGenre;
            this.Email = user.Email;
            this.ReviewCount = user.ReviewCount;
        }

        [Key]
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Favorite Game")]
        public string FavoriteGame { get; set; }

        [Required]
        [Display(Name = "Favorite Genre")]
        public Genre FavoriteGenre { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [Display(Name ="Total Reviews")]
        public int ReviewCount { get; set; }
    }

    public class CreateUserViewModel
    {
        public CreateUserViewModel() { }

        public CreateUserViewModel(ApplicationUser user)
        {
            this.UserName = user.UserName;
            this.FavoriteGame = user.FavoriteGame;
            this.FavoriteGenre = user.FavoriteGenre;
            this.Email = user.Email;
            this.ReviewCount = user.ReviewCount;
            this.Password = user.PasswordHash;
        }

        [Key]
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Favorite Game")]
        public string FavoriteGame { get; set; }

        [Required]
        [Display(Name = "Favorite Genre")]
        public Genre FavoriteGenre { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Total Reviews")]
        public int ReviewCount { get; set; }
    }

    public class RoleViewModel
    {
        public RoleViewModel() { }

        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        public string Name { get; set; }
    }


}
