using CommonTestUtilities.Requests;
using Shouldly;
using TarefasCrud.Application.UseCases.Tasks;
using TarefasCrud.Communication.Requests;
using TarefasCrud.Exceptions;

namespace Validators.Test.Task;

public class TaskValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new TaskValidator();
        var request = RequestTaskJsonBuilder.Build();
        var result = validator.Validate(request);
        result.IsValid.ShouldBe(true);
    }    
    
    [Fact]
    public void Error_Title_Empty()
    {
        var validator = new TaskValidator();
        var request = RequestTaskJsonBuilder.Build();
        request.Title = string.Empty;
        
        var result = validator.Validate(request);
        result.IsValid.ShouldBe(false);
        result.Errors.ShouldHaveSingleItem(); 
        result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.TASK_TITLE_EMPTY));
    }    
    
    [Fact]
    public void Error_Description_Exceeds_Character_Limit()
    {
        var validator = new TaskValidator();
        var request = RequestTaskJsonBuilder.Build(descriptionChar: 200);
        
        var result = validator.Validate(request);
        result.IsValid.ShouldBe(false);
        result.Errors.ShouldHaveSingleItem(); 
        result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.DESCRIPTION_EXCEEDS_LIMIT_CHARACTERS));
    }
}