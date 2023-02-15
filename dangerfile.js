const {danger, warn} = require('danger')


// Ensure the title is in the correct format
let title = danger.github.pr.title;
if (title[0] !== title[0].toUpperCase()) {
  fail("PR title needs to start with an uppercase letter.")
}

// Every PR needs someone assigned to it
if (!danger.github.pr.assignee) {
  fail("This pull request needs an assignee, and optionally include any reviewers.")
}

// No PR is too small to include a description of why you made a change
if (danger.github.pr.body.length < 10) {
  warn('Please include a description of your PR changes.');
}
