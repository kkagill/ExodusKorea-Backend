using FluentValidation;

namespace ExodusKorea.Model.ViewModels.Validations
{
    public class VideoCommentVMValidator : AbstractValidator<VideoCommentVM>
    {
        public VideoCommentVMValidator()
        {
            RuleFor(s => s.Comment).Length(5, 2500).WithMessage("최소 5글자에서 500글자 사이어야합니다.");
        }
    }
}