// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
(function () {
	var BUFFER_MS = 2000;
	var clickableSelector = [
		"a[href]",
		"button",
		"input[type='button']",
		"input[type='submit']",
		"input[type='reset']",
		"[role='button']",
		"summary"
	].join(",");
	var isBuffering = false;
	var activeTarget = null;

	function isDisabled(element) {
		return element.matches(":disabled") || element.classList.contains("disabled");
	}

	function isSubmitControl(element) {
		if (!(element instanceof HTMLElement)) {
			return false;
		}

		if (element.tagName === "BUTTON") {
			return element.getAttribute("type") !== "button";
		}

		if (element.tagName === "INPUT") {
			var inputType = (element.getAttribute("type") || "").toLowerCase();
			return inputType === "submit" || inputType === "image";
		}

		return false;
	}

	function setBufferState(target) {
		isBuffering = true;
		activeTarget = target;
		document.body.classList.add("is-buffering");
		if (activeTarget) {
			activeTarget.classList.add("buffering-target");
		}
	}

	function clearBufferState() {
		isBuffering = false;
		document.body.classList.remove("is-buffering");
		if (activeTarget) {
			activeTarget.classList.remove("buffering-target");
		}
		activeTarget = null;
	}

	function runAfterBuffer(callback) {
		window.setTimeout(function () {
			clearBufferState();
			callback();
		}, BUFFER_MS);
	}

	function navigateAfterBuffer(anchor) {
		var href = anchor.getAttribute("href");
		if (!href || href === "#") {
			return;
		}

		if (anchor.target && anchor.target.toLowerCase() === "_blank") {
			window.open(anchor.href, "_blank", anchor.rel || "noopener,noreferrer");
			return;
		}

		window.location.assign(anchor.href);
	}

	function submitFormAfterBuffer(form, submitter) {
		form.dataset.bufferBypass = "true";

		if (submitter && typeof form.requestSubmit === "function") {
			form.requestSubmit(submitter);
			return;
		}

		form.submit();
	}

	function replayClickAfterBuffer(target) {
		target.dataset.bufferBypass = "true";
		target.click();
	}

	document.addEventListener(
		"click",
		function (event) {
			if (event.defaultPrevented || event.button !== 0) {
				return;
			}

			if (event.metaKey || event.ctrlKey || event.shiftKey || event.altKey) {
				return;
			}

			var target = event.target.closest(clickableSelector);
			if (!target || isDisabled(target)) {
				return;
			}

			if (target.dataset.bufferBypass === "true") {
				delete target.dataset.bufferBypass;
				return;
			}

			if (isBuffering) {
				event.preventDefault();
				event.stopImmediatePropagation();
				return;
			}

			event.preventDefault();
			event.stopImmediatePropagation();
			setBufferState(target);

			runAfterBuffer(function () {
				if (target.tagName === "A") {
					navigateAfterBuffer(target);
					return;
				}

				var form = target.closest("form");
				if (form && isSubmitControl(target)) {
					submitFormAfterBuffer(form, target);
					return;
				}

				replayClickAfterBuffer(target);
			});
		},
		true
	);

	document.addEventListener(
		"submit",
		function (event) {
			var form = event.target;
			if (!(form instanceof HTMLFormElement)) {
				return;
			}

			if (form.dataset.bufferBypass === "true") {
				delete form.dataset.bufferBypass;
				return;
			}

			if (isBuffering) {
				event.preventDefault();
				return;
			}

			event.preventDefault();
			setBufferState(form);

			runAfterBuffer(function () {
				submitFormAfterBuffer(form, event.submitter || null);
			});
		},
		true
	);
})();
