$(document).ready(function() {
	initializeHandlers();
	initializeQuestions();
    startTest();
});

function initializeHandlers() {
	$(".btn-prev").click(function() {
		showQuestion(--currentQuestionIdx);
	});
	
	$(".btn-next").click(function() {
		showQuestion(++currentQuestionIdx);
	});
}

var hamQuestions = null;
var currentQuestionIdx = 0;
function initializeQuestions() {
	hamQuestions = hamQuestions1;
}

function startTest() {
	showQuestion(currentQuestionIdx);
}

function showQuestion(questionIdx) {
	var question = hamQuestions[questionIdx];
	var questionHtml = $(".ham-question-template").clone();
	questionHtml.removeClass("ham-question-template");
	
	questionHtml.find(".question-text").html((questionIdx + 1).toString() + ". " + question.question);
	
	var answersHtml = question.answers.map((ans, idx) => {
			var answerHtml = questionHtml.find(".answer").clone();
			var id = "chkAnswer" + idx;
			
			answerHtml.find(".form-check-input")
			.attr("id", id)
			.attr("iscorrect", (ans.isCorrect ? "yes" : "no"));
			
			answerHtml.find(".form-check-label").attr("for", id);
			var ansLabelPrefix = "";
			switch(idx) {
				case 0: ansLabelPrefix = "А."; break;
				case 1: ansLabelPrefix = "Б."; break;
				case 2: ansLabelPrefix = "В."; break;
				case 3: ansLabelPrefix = "Г."; break;
			}
			answerHtml.find(".form-check-label").html(ansLabelPrefix + " " + ans.answer);
			return answerHtml;
		}
	);
	
	var answersPanel = questionHtml.find(".answers");
	answersPanel.empty();
	answersHtml.forEach(ansHtml => answersPanel.append(ansHtml));
	
	questionHtml.css("display", "");
	$(".current-question").empty();
	$(".current-question").append(questionHtml);
	
	refreshButtons();
}

function refreshButtons() {
	$(".btn-prev").prop('disabled', currentQuestionIdx == 0);
	$(".btn-next").prop('disabled', currentQuestionIdx == hamQuestions.length - 1);
}

